using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RecoveryManager: IRecoveryManager
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        
        private IRecoveryService _recoveryService { get; set; }

        private IMailService _mailService { get; set; }
        public RecoveryManager(ISqlDAO sqlDAO, ILogService logService, IRecoveryService recoveryService, IMailService mailService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _recoveryService = recoveryService;
            _mailService = mailService;
        }


        public async Task<string> SendRecoveryEmail(string email, string baseurl, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
           
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                //Check the status of account
                Tuple<IAccount, string> account = await _recoveryService.GetAccountAsync(email, authorizationLevel, cancellationToken);
                if (cancellationToken.IsCancellationRequested) //No rollback necessary if canceled
                    return "499";                               // Request cancelled
                if(account.Item1 == null)
                {
                    // Account doesn't exist, return error code
                    return account.Item2;
                }

                // Check if account is disabled
                Tuple<bool, string> IsDisabled = await _recoveryService.IsAccountDisabledAsync(account.Item1, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    // Cancellation requested....no rollback necessary 
                    return "499";           // 499 - request canceled
                    
                }

                if (!IsDisabled.Item1)
                {
                    if (IsDisabled.Item2 != "200")
                        return IsDisabled.Item2;
                    else
                        return "409";               // 409 conflict, user is already enabled. no need to enable
                }

                // Create the recovery link
                Tuple<IRecoveryLink, string> recoveryLink = await _recoveryService.CreateRecoveryLinkAsync(account.Item1, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    string rollBackresult = await _recoveryService.RemoveRecoveryLinkAsync(recoveryLink.Item1);
                    if (rollBackresult == "200")
                        return "499";
                    else
                        return "503"; //503 Service Unavailable -Roll back failed
                }

                if (recoveryLink.Item1 == null || recoveryLink.Item2 != "200")
                {
                    // Recovery link doesn't exist
                    // TO DO: NEED TO RETURN CODE THAT NOTIFIES THAT THEY'VE REACHED 5 CALENDAR LIMIT
                    
                    return recoveryLink.Item2;
                }

                //Send Recovery Link --> no possible rollback at this point but can still cancel
                string mailResults = await _mailService.SendRecoveryAsync(email, baseurl+recoveryLink.Item1.GUIDLink, cancellationToken);
                return mailResults;
            }
            catch (OperationCanceledException ex)
            {
                // Nothing to rollback
                return "499";
            }
            catch (Exception ex)
            {
                return "500";
            }
        }
    }
}
