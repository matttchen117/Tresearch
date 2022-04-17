using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class UserManagementManager
    {
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMailService _mailService { get; set; }
        private IUserManagementService _userManagementService { get; set; }
        private IAccountVerificationService _accountVerificationService { get; set; }
        private IMessageBank _messageBank { get; set; }

        private string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm/guid=";
        public UserManagementManager(ISqlDAO sqlDAO, ILogService logService, IAccountVerificationService accountVerificationService, IMailService mailService, IUserManagementService userManagementService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _mailService = mailService;
            _accountVerificationService = accountVerificationService;
            _userManagementService = userManagementService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        public async Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if user has a token already. User should not be logged in
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount threadAccount = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(threadAccount, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    Tuple<IConfirmationLink, string> resultCreateAccount = await _userManagementService.CreateAccountAsync(account, cancellationToken);
                    IConfirmationLink confirmationLink = resultCreateAccount.Item1;

                    if (confirmationLink == null || !resultCreateAccount.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                            return await _messageBank.GetMessage(IMessageBank.Responses.accountCreateFail);

                    string result = await _mailService.SendConfirmationAsync(account.Username, baseUrl+confirmationLink.GUIDLink, cancellationToken);

                    return result;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }

            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }


        public async Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if user has a token already. User should not be logged in
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount threadAccount = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(threadAccount, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    string result = await _userManagementService.UpdateAccountAsync(account, updatedAccount, cancellationToken);

                    return result;

                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }

            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if user has a token already. User should not be logged in
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount threadAccount = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(threadAccount, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    string result = await _userManagementService.DeleteAccountAsync(account, cancellationToken);

                    return result;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
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

                //Check if user has a token already. User should not be logged in
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount threadAccount = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(threadAccount, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    string result = await _userManagementService.EnableAccountAsync(account, cancellationToken);

                    return result;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
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

                //Check if user has a token already. User should not be logged in
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount threadAccount = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(threadAccount, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    string result = await _userManagementService.DisableAccountAsync(account, cancellationToken);

                    return result;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }

            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }
    }
}
