using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RegistrationManager : IRegistrationManager
    {
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMailService _mailService { get; set; }
        private IRegistrationService _registrationService { get; set; }
        private IMessageBank _messageBank { get; set; }

        private string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm/guid=";
        public RegistrationManager(ISqlDAO sqlDAO, ILogService logService, IRegistrationService accountService, IMailService mailService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _mailService = mailService;
            _registrationService = accountService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     CreateAndSendConfirmationAsync(string email, string passphrase, string authorizationLevel)
        /// </summary>
        /// <param name="email">Email of user's new account. Must not be hashed</param>
        /// <param name="passphrase">Passphrase is already hashed from client</param>
        /// <param name="authorizationLevel">Authorization Level of user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String result</returns>
        public async Task<string> CreateAndSendConfirmationAsync(string email, string passphrase, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if user has a token already. User should not be logged in
                if (Thread.CurrentPrincipal.Identity.Name == "guest")
                {
                    //Hash email Uusing pbkdf2
                    string resultHashEmail = await _registrationService.HashValueAsync(email+authorizationLevel, cancellationToken);
              
                    //Create an account in UserAccount Tables
                    Tuple<int, string> resultCreateAccount = await _registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationToken);

                    //Create a UserID, UserRole and UserHash in UserHashTable
                    string resultInsertUserHashTable = await _registrationService.CreateHashTableEntry(resultCreateAccount.Item1, resultHashEmail, cancellationToken);

                    //Check if HashTable was succesfully added
                    if (!resultInsertUserHashTable.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        return resultInsertUserHashTable;


                    //Check if account  was created
                    if (resultCreateAccount.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return resultCreateAccount.Item2;

                    //Create OTP 
                    string resultCreateOTP = await _registrationService.CreateOTPAsync(email, authorizationLevel, cancellationToken);

                    if (resultCreateOTP != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return resultCreateOTP;

                    //Create UserAccount Confirmation Link
                    Tuple<IConfirmationLink, string> confirmationLinkResult = await _registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationToken).ConfigureAwait(false);
                    
                    //Check if confirmation link was successfully created
                    if(!confirmationLinkResult.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        return confirmationLinkResult.Item2;

                    IConfirmationLink confirmationLink = confirmationLinkResult.Item1;
                    
                    //Send confirmation link to user
                    string linkUrl = $"{baseUrl}{confirmationLink.GUIDLink.ToString()}";
                    string mailResult = await _mailService.SendConfirmationAsync(email, linkUrl).ConfigureAwait(false);

                    return mailResult;

                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.alreadyAuthenticated);
                }    


                
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of 
                throw;
            }

            catch(Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
            
        }

        public async Task<string> ConfirmAccountAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IConfirmationLink, string> confirmationLink = await _registrationService.GetConfirmationLinkAsync(guid).ConfigureAwait(false);
                
                if (confirmationLink.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return confirmationLink.Item2;

                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                //Check if confirmation link is valid
                if (!IsConfirmationLinkInvalid(confirmationLink.Item1))
                {
                        return _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkExpired).Result;
                }

                IConfirmationLink linkInfo = confirmationLink.Item1;
                string confirmResult = await _registrationService.ConfirmAccountAsync(linkInfo.Username, linkInfo.AuthorizationLevel, cancellationToken).ConfigureAwait(false);

                if (cancellationToken.IsCancellationRequested)
                {
                    //PEFORM ROLLBACK
                }

                if (confirmResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return confirmResult;

                string removeResult = await _registrationService.RemoveConfirmationLinkAsync(confirmationLink.Item1, cancellationToken).ConfigureAwait(false);

                if (cancellationToken.IsCancellationRequested)
                {
                    //Rollback confirm
                    //Rollback removal
                }
                
                if (removeResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkRemoveFail).Result;
                else
                    return confirmResult;
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

        public async Task<string> ResendConfirmation(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                Tuple<IConfirmationLink, string> confirmationLink = await _registrationService.GetConfirmationLinkAsync(guid).ConfigureAwait(false);

                if (confirmationLink.Item2 != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                    return confirmationLink.Item2;

                // Remove confirmation link
                string removeOld = await _registrationService.RemoveConfirmationLinkAsync(confirmationLink.Item1, cancellationToken).ConfigureAwait(false);

                if (!removeOld.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    return removeOld;

                if (cancellationToken.IsCancellationRequested && removeOld.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                {
                    string rollbackCreate = await _registrationService.CreateConfirmationAsync(confirmationLink.Item1);
                    if (rollbackCreate.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        throw new OperationCanceledException();
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.generic);
                }

                Tuple<IConfirmationLink, string> newConfirmationLink = await _registrationService.CreateConfirmationAsync(confirmationLink.Item1.Username, confirmationLink.Item1.AuthorizationLevel, cancellationToken).ConfigureAwait(false);

                if (!removeOld.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    return newConfirmationLink.Item2;

                if (cancellationToken.IsCancellationRequested && newConfirmationLink.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                {
                    string rollbackCreate = await _registrationService.CreateConfirmationAsync(confirmationLink.Item1);
                    if (!rollbackCreate.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        return await _messageBank.GetMessage(IMessageBank.Responses.generic);
                    string rollbackRemove = await _registrationService.RemoveConfirmationLinkAsync(newConfirmationLink.Item1);
                    if (!rollbackCreate.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        return await _messageBank.GetMessage(IMessageBank.Responses.generic);
                    else
                        throw new OperationCanceledException();
                }

                //Beyond point of no return cannot cancel
                string linkUrl = $"{baseUrl}{newConfirmationLink.Item1.GUIDLink.ToString()}";
                string mailResult = await _mailService.SendConfirmationAsync(confirmationLink.Item1.Username, linkUrl).ConfigureAwait(false);

                return mailResult;


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

        public bool IsConfirmationLinkInvalid(IConfirmationLink confirmationLink)
        {
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime yesterday = now.AddDays(-1);
            if (confirmationLink.TimeCreated <= now && confirmationLink.TimeCreated >= yesterday)
                return true;
            else
                return false;
        }
    }
}
