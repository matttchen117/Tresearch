using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RecoveryManager: IRecoveryManager
    {
        private IAccountVerificationService _accountVerificationService { get; set; }       //Use to verify account exists
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private IRecoveryService _recoveryService { get; set; }

        private IMailService _mailService { get; set; }
        public RecoveryManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IRecoveryService recoveryService, IAccountVerificationService accountVerificationService, IMailService mailService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _recoveryService = recoveryService;
            _mailService = mailService;
            _accountVerificationService = accountVerificationService;
            
        }

        public async Task<string> SendRecoveryEmailAsync(string email, string baseurl, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
           
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if user has a token already. User should not be logged in.
                if (Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IAccount account = new UserAccount(email, authorizationLevel);

                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                    if(resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.notEnabled)))
                    {
                        //Check if account has less than five links
                        int linkTotal = await _recoveryService.GetRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken);
                        if (linkTotal >= 5)
                            return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkLimitReached).Result;

                        // Create the recovery link
                        Tuple<IRecoveryLink, string> recoveryLink = await _recoveryService.CreateRecoveryLinkAsync(account, cancellationToken);

                        //Send Recovery Link --> no possible rollback at this point but can still cancel
                        string link = baseurl +  recoveryLink.Item1.GUIDLink.ToString();
                        string mailResults = await _mailService.SendRecoveryAsync(email, link, cancellationToken);
                        return mailResults;
                    }
                    else
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrAuthorized);
                    }
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.alreadyAuthenticated);
                }       
            }
            catch (OperationCanceledException)
            {
                // Nothing to rollback
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> EnableAccountAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                string guid = url.Substring(url.LastIndexOf('=') + 1);
                
                cancellationToken.ThrowIfCancellationRequested();

                //Get recoverylink from database
                Tuple<IRecoveryLink, string> recoveryLink = await _recoveryService.GetRecoveryLinkAsync(guid, cancellationToken);

                if (recoveryLink.Item2 != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return recoveryLink.Item2;

                

                //Check if recovery link is older than 24 hours
                if (!recoveryLink.Item1.isValid())
                    return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkExpired).Result;
                //Enable UserAccount
                string enableResult = await _recoveryService.EnableAccountAsync(recoveryLink.Item1.Username, recoveryLink.Item1.AuthorizationLevel, cancellationToken);

                if (enableResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return enableResult;

                //Remove old link and increment
                string removeResult = await _recoveryService.RemoveRecoveryLinkAsync(recoveryLink.Item1, cancellationToken).ConfigureAwait(false);

                if (removeResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return removeResult;
                
                string incrementResult = await _recoveryService.IncrementRecoveryLinkCountAsync(recoveryLink.Item1.Username, recoveryLink.Item1.AuthorizationLevel, cancellationToken).ConfigureAwait(false); ;
                
                if (cancellationToken.IsCancellationRequested)
                {
                    string rollBack = await _recoveryService.DecrementRecoveryLinkCountAsync(recoveryLink.Item1.Username, recoveryLink.Item1.AuthorizationLevel);
                    if (rollBack != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result;
                    else
                        throw new OperationCanceledException();
                }

                if (incrementResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return incrementResult;

                return incrementResult;

            }
            catch (OperationCanceledException)
            {
                // Nothing to rollback
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }
    }
}
