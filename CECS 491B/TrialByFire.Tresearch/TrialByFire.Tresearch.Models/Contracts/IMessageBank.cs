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
            unhandledException,
            operationCancelled,

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

            //Rating
            userRateSuccess,
            userRateFail,
            getRateSuccess,
            getRateFail,

            jwtValidationSuccess,
            jwtValidationFail,

            refreshSessionSuccess,
            refreshSessionFail,
            refreshSessionNotAllowed,

            accountUpdateSuccess,
            accountUpdateFail,
            accountEnableSuccess,
            accountDisableSuccess,

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
            nodeNotFound,
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
            createNodeFail,
            nodeAlreadyExists,
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

            getAdminsSuccess,
            accountDeletionSuccess,
            lastAdminFail,
            accountAlreadyDeleted,
            accountDeleteFail,
            verificationFailure,

            nodeTagNodeDoesNotExist,
            tagAlreadyExist,
            tagDoesNotExist,
            createNodeSuccess,
        }
        public Task<string> GetMessage(Responses response);
    }
}
