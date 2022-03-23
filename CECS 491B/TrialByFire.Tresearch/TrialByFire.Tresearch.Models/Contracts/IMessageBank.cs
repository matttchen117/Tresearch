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
            badNameOrPass,
            badNameOrOTP,
            badEmail,
            tooManyFails,
            otpExpired,
            notAuthenticated,
            notConfirmed,
            alreadyAuthenticated,
            notAuthorized,
            alreadyEnabled,
            recoveryLinkLimitReached,
            accountNotFound,
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
            recoveryLinkExists,
            recoveryLinkExpired,
            otpFail,
            databaseFail,
            logoutFail,
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
