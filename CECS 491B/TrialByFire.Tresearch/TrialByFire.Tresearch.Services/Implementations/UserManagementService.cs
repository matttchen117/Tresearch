using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;



namespace TrialByFire.Tresearch.Services.Implementations
{
    public class UserManagementService: IUserManagementService
    {
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        public UserManagementService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }


        public async Task<Tuple<IConfirmationLink, string>> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink failed = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                Tuple<int, string> resultCreateAccount = await _sqlDAO.CreateAccountAsync(account, cancellationToken);
                int accountID = resultCreateAccount.Item1;

                string hashed = await HashValueAsync(account.Username+account.AuthorizationLevel);
                
                string resultCreateHash = await _sqlDAO.CreateUserHashAsync(accountID, hashed, cancellationToken);

                IConfirmationLink confirmationLink = new ConfirmationLink(hashed, account.AuthorizationLevel, Guid.NewGuid(), DateTime.Now);

                string resultCreateOtp = await _sqlDAO.CreateOTPAsync(account.Username, account.AuthorizationLevel, 0, cancellationToken);

                string result = await _sqlDAO.CreateConfirmationLinkAsync(confirmationLink, cancellationToken);

                return Tuple.Create(confirmationLink, result);
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(failed, await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch(Exception ex)
            {
                return Tuple.Create(failed, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message);
            }
        }

        public async Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string result = await _sqlDAO.UpdateAccountAsync(account, updatedAccount, cancellationToken);

                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string result = await _sqlDAO.DeleteAccountAsync(account, cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> EnableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string results = await _sqlDAO.EnableAccountAsync(account.Username, account.AuthorizationLevel, cancellationToken);

                return results;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }
        public async Task<string> DisableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string results = await _sqlDAO.DisableAccountAsync(account.Username, account.AuthorizationLevel, cancellationToken);

                return results;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> HashValueAsync(string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(value, 0, iterations: 10000, HashAlgorithmName.SHA512);
                return string.Join(string.Empty, Array.ConvertAll(pbkdf2.GetBytes(64), b => b.ToString("X2")));
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
