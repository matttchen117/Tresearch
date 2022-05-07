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
            operationTimeExceeded,

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
            logTimeExceeded,
            logRollback,

            nodeSearchSuccess,
            noSearchInput,

            /// <summary>
            /// Node given for creation is empty
            /// </summary>
            noNodeInput,

            /// <summary>
            /// ParentNodeID(s) edited of the given Nodes
            /// </summary>
            editParentSuccess,
            /// <summary>
            /// The List of NodeIDs for the parent to be edited is empty
            /// </summary>
            noEditParentNodeInput,
            /// <summary>
            /// The ID given for the Node's ParentID to be assigned to does not exist
            /// </summary>
            noTargetParent,

            /// <summary>
            /// Tag added to node(s)
            /// </summary>
            tagAddSuccess,
            /// <summary>
            /// Tag removed from node(s)
            /// </summary>
            tagRemoveSuccess,
            /// <summary>
            /// Tag created in tag bank
            /// </summary>
            tagCreateSuccess,
            /// <summary>
            /// Tag deleted from tag bank
            /// </summary>
            tagDeleteSuccess,
            /// <summary>
            /// Tag bank retrieved
            /// </summary>
            tagGetSuccess,
            /// <summary>
            /// Tag bank retrieval failed
            /// </summary>
            tagRetrievalFail,
            /// <summary>
            /// Tag already exists in tag bank
            /// </summary>
            tagDuplicate,
            /// <summary>
            /// Tag does not exist in tag bank
            /// </summary>
            tagNotFound,
            /// <summary>
            /// Invalid tag count
            /// </summary>
            tagCountInvalid,
            /// <summary>
            /// Invalid tag name
            /// </summary>
            tagNameInvalid,

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
            deleteNodeFail,
            updateNodeFail,
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
            deleteNodeSuccess,
            getNodesSuccess,

            copyNodeSuccess,
            copyNodeFailure,
            copyNodeError,
            copyNodeEmptyError,
            copyNodeMistmatchError,

            isLeaf,
            isNotLeaf,

            pasteNodeSuccess,
            pasteNodeFailure,
            pasteNodeError,
            pasteNodeEmptyError,
            pasteNodeMistmatchError,
            notAuthorizedToPasteTo,

            privateNodeSuccess,
            privateNodeFailure,

            publicNodeSuccess,
            publicNodeFailure,


        }
        public Task<string> GetMessage(Responses response);
    }
}

