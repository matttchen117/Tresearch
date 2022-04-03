using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RegistrationManager : IRegistrationManager
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public IMailService _mailService { get; set; }
        public IRegistrationService _registrationService { get; set; }
        public IValidationService _validationService { get; set; }
        public IMessageBank _messageBank { get; set; }
        private int linkActivationLimit = 24;
        public RegistrationManager(ISqlDAO sqlDAO, ILogService logService, IRegistrationService accountService, IMailService mailService, IValidationService validationService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _mailService = mailService;
            _registrationService = accountService;
            _validationService = validationService;
            _messageBank = messageBank;
        }

        public async Task<string> CreateAndSendConfirmationAsync(string email, string passphrase, string authorizationLevel, string baseUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string resultHashed = await _registrationService.HashValueAsync(email, cancellationToken);

                if (resultHashed == "")
                    throw new Exception();

                string resultHashTable = await _registrationService.CreateHashTableEntry(email, resultHashed, cancellationToken);
                
                string resultCreate = await _registrationService.CreateAccountAsync(resultHashed, passphrase, authorizationLevel, cancellationToken);

                
                //Check if request is cancelled and acccount was already created
                if(cancellationToken.IsCancellationRequested && resultCreate == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform account removal
                }

                //Check if account  was created
                if (resultCreate != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return resultCreate;

                Tuple<IConfirmationLink, string> confirmationLink = await _registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationToken).ConfigureAwait(false);

                
                //Check if request is cancelled and confirmation link was already created
                if(cancellationToken.IsCancellationRequested && confirmationLink.Item2 == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform account removal
                    string rollbackResult = await _registrationService.RemoveConfirmationLinkAsync(confirmationLink.Item1, cancellationToken).ConfigureAwait(false);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }

                if (confirmationLink.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return confirmationLink.Item2;

                //Beyond point of no return cannot cancel
                string linkUrl = $"{baseUrl}{confirmationLink.Item1.GUIDLink.ToString()}";
                string mailResult = await _mailService.SendConfirmationAsync(email, linkUrl).ConfigureAwait(false);

                return mailResult;
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

        public async Task<string> ResendConfirmation(string guid, string baseUrl, CancellationToken cancellationToken = default(CancellationToken))
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
