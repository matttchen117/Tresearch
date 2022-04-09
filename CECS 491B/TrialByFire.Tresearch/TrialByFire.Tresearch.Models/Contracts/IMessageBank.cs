using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IMessageBank
    {
        public Dictionary<string, string> SuccessMessages { get; }
        public Dictionary<string, string> ErrorMessages { get; }
        public enum Responses
        {
            generic,

            databaseConnectionFail,

            verifySuccess,
            notEnabled,
            notConfirmed,
            accountNotFound,

            authenticationSuccess,
            otpExpired,
            tooManyFails,
            badNameOrOTP,
            duplicateAccountData,
            authenticationRollback,

            storeOTPSuccess,
            otpClaimNotFound,
            duplicateOTPClaimData,
            storeOTPRollback,

            logoutSuccess,
            unknownRole,
            logoutFail,
            logoutRollback,

            logSuccess,
            logFail,
            logRollback,

            tagAddSuccess,
            tagRemoveSuccess,
            tagCreateSuccess,
            tagDeleteSuccess,
            tagGetSuccess,
            tagRetrievalFail,
            tagDuplicate,
            tagNotFound,

            nodeNotFound,

            badNameOrPass,
            badEmail,
            notAuthenticated,
            alreadyAuthenticated,
            notAuthorized,
            accountAlreadyCreated,
            accountConfirmedFail,
            accountUnconfirmedFail,
            accountAlreadyConfirmed,
            accountAlreadyUnconfirmed,
            alreadyEnabled,
            recoveryLinkLimitReached,
            notFoundOrEnabled,
            notFoundOrAuthorized,
            recoveryLinkNotFound,
            cancellationRequested,
            confirmationLinkCreateFail,
            confirmationLinkRemoveFail,
            confirmationLinkNotFound,
            confirmationLinkExpired,
            confirmationLinkExists,
            cookieFail,
            sendEmailFail,
            accountCreateFail,
            accountDisableFail,
            accountEnableFail,
            recoveryLinkRemoveFail,
            recoveryLinkCreateFail,
            recoveryLinkExists,
            recoveryLinkExpired,
            otpFail,
            databaseFail,
            rollbackFailed,
            createdNodesExists,
            createdNodeNotExist,
            createdNodeNotInserted,
            dailyLoginsExists,
            dailyLoginNotInserted,
            dailyLoginNotExist,
            
            topSearchExists,
            topSearchNotInserted,
            topSearchNotExist,
            dailyRegistrationExists,
            dailyRegistrationNotInserted,
            dailyRegistrationNotExist,
            storeLogFail
            
            
        }
        public Task<string> GetMessage(Responses response);
    }
}
