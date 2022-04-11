using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using System.Security.Cryptography;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     Reigstration service to call sql dao layer. Handles user account registration and confirmation.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }

        private int linkActivationLimit = 24;

        public RegistrationService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     CreateAccountAsync(email, passphrase, authorizationlevel)
        ///
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passphrase"></param>
        /// <param name="authorizationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<int, string>> CreateAccountAsync(string email, string passphrase, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IAccount account = new UserAccount(email, passphrase, authorizationLevel, true, false);
                Tuple<int, string> createResult = await _sqlDAO.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if(cancellationToken.IsCancellationRequested && createResult.Item2 == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform Rollback

                }

                return createResult;
            }
            catch (OperationCanceledException)
            {
                //Nothing to rollback
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, "500: Server: " + ex.Message);
            }
        }

        public async Task<string> CreateOTPAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
               
                string createResult = await _sqlDAO.CreateOTPAsync(email, authorizationLevel, 0, cancellationToken).ConfigureAwait(false);

                if (cancellationToken.IsCancellationRequested && createResult == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform Rollback

                }

                return createResult;
            }
            catch (OperationCanceledException)
            {
                //Nothing to rollback
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        /// <summary>
        ///     CreateConfirmationAsync(email, authorizationLevel)
        ///         Creates Confirmation link
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="authorizationLevel">Authorization Level of user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Confirmation link and string status code</returns>
        public async Task<Tuple<IConfirmationLink, string>> CreateConfirmationAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                Guid guid = Guid.NewGuid();
                IConfirmationLink confirmationLink = new ConfirmationLink(email, authorizationLevel, guid, DateTime.Now);

                string result = await _sqlDAO.CreateConfirmationLinkAsync(confirmationLink, cancellationToken).ConfigureAwait(false);

                if (result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return Tuple.Create(nullLink, result);

                if(cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.RemoveConfirmationLinkAsync(confirmationLink);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result);
                    else
                        throw new OperationCanceledException();
                }
                return Tuple.Create(confirmationLink, result);
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex.Message);

            }
        }

        public async Task<string> CreateConfirmationAsync(IConfirmationLink link, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {

                string result = await _sqlDAO.CreateConfirmationLinkAsync(link, cancellationToken).ConfigureAwait(false);

                if (result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return result;

                if (cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.RemoveConfirmationLinkAsync(link);
                    if (rollbackResult != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                        return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed);
                    else
                        throw new OperationCanceledException();
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;

            }
        }

        public async Task<string> ConfirmAccountAsync(string username, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string results = await _sqlDAO.UpdateAccountToConfirmedAsync(username, authorizationLevel, cancellationToken);
                if(cancellationToken.IsCancellationRequested && results == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Implement rollback
                    string rollbackResults = await _sqlDAO.UpdateAccountToUnconfirmedAsync(username, authorizationLevel, cancellationToken).ConfigureAwait(false);
                    if (rollbackResults != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return results;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary (or taken care of)
                throw;
            }
            catch(Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IConfirmationLink, string> confirmationLink = await _sqlDAO.GetConfirmationLinkAsync(guid).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return confirmationLink;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex.Message);
            }
        }

        public async Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.RemoveConfirmationLinkAsync(confirmationLink, cancellationToken).ConfigureAwait(false);
                if(cancellationToken.IsCancellationRequested && result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.CreateConfirmationLinkAsync(confirmationLink).ConfigureAwait(false);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<string> HashValueAsync(string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(value, 0, iterations: 10000, HashAlgorithmName.SHA512);
                return string.Join(string.Empty, Array.ConvertAll(pbkdf2.GetBytes(64), b => b.ToString("X2")));
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public async Task<string> CreateHashTableEntry(int ID, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.CreateUserHashAsync(ID, hashedEmail, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }
    }
}
