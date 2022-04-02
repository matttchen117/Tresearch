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

            badNameOrPass,
            badEmail,
            notAuthenticated,
            alreadyAuthenticated,
            notAuthorized,
            alreadyEnabled,
            recoveryLinkLimitReached,
            notFoundOrEnabled,
            notFoundOrAuthorized,
            recoveryLinkNotFound,
            cancellationRequested,
            cookieFail,
            sendEmailFail,
            accountDisableFail,
            accountEnableFail,
            recoveryLinkRemoveFail,
            recoveryLinkCreateFail,
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
            storeLogFail,
        }
        public Task<string> GetMessage(Responses response);
    }
}
