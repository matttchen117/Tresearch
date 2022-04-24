using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using static TrialByFire.Tresearch.Models.Contracts.IMessageBank;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class MessageBank : IMessageBank
    {
        public Dictionary<string, string> SuccessMessages { get; }
        public Dictionary<string, string> ErrorMessages { get; }

        public MessageBank()
        {
            SuccessMessages = InitializeSuccessMessages();
            ErrorMessages = InitializeErrorMessages();
        }

        private Dictionary<string, string> InitializeSuccessMessages()
        {
            Dictionary<string, string> successMessages = new Dictionary<string, string>();
            successMessages.Add("generic", "200: Server: success");
            return successMessages;
        }

        public async Task<string> GetMessage(Responses response)
        {
            switch (response)
            {
                case Responses.generic:
                    return "200: Server: success";
                case Responses.unhandledException:
                    return "500: Server: Unhandled Exception: {0}, Operation: {1}";
                case Responses.operationCancelled:
                    return "400: Server: Operation Cancelled Exception: ";

                case Responses.databaseConnectionFail:
                    return "503: Database: Unable to connect to database.";

                case Responses.verifySuccess:
                    return "200: Server: UserAccount Verification success.";
                case Responses.notEnabled:
                    return "401: Database: UserAccount disabled. Perform account recovery or contact system admin.";
                case Responses.notConfirmed:
                    return "401: Database: Please confirm your account before attempting to login.";
                case Responses.accountNotFound:
                    return "500: Database: The UserAccount was not found.";

                case Responses.authenticationSuccess:
                    return "200: Server: Authentication success.";
                case Responses.badNameOrOTP:
                    return "400: Data: Invalid Username or OTP. Please try again.";
                case Responses.tooManyFails:
                    return "400: Database: Too many fails have occurred. The account has been disabled.";
                case Responses.otpExpired:
                    return "400: Data: The OTP has expired. Please request a new one.";
                case Responses.duplicateAccountData:
                    return "500: Database: Duplicate UserAccount found.";
                case Responses.authenticationRollback:
                    return "400: Database: Authentication rollback occurred.";

                case Responses.storeOTPSuccess:
                    return "200: Server: StoreOTP success.";
                case Responses.otpClaimNotFound:
                    return "500: Database: The OTP Claim was not found.";
                case Responses.duplicateOTPClaimData:
                    return "500: Database: Duplicate OTP Claim found.";
                case Responses.storeOTPRollback:
                    return "400: Database: StoreOTP rollback occurred.";


                case Responses.logoutSuccess:
                    return "200: Server: Logout success.";
                case Responses.unknownRole:
                    return "400: Server: Unknown role used.";
                case Responses.logoutFail:
                    return "503: Server: Logout failed.";
                case Responses.logoutRollback:
                    return "400: Database: Logout rollback occurred.";

                case Responses.logSuccess:
                    return "200: Server: Log success.";
                case Responses.logFail:
                    return "503: Server: Log failed.";
                case Responses.logTimeExceeded:
                    return "503: Datastore: Logging exceeded time: ";
                case Responses.logRollback:
                    return "400: Database: Log rollback occurred.";
                case Responses.nodeSearchSuccess:
                    return "200: Server: Node search success. Search: {0}, Tags: {1}, FilterByRating: {2}, FilterByTime: {3}";

                
                case Responses.tagAddSuccess:
                    return "200: Server: Tag added to node(s).";
                case Responses.tagRemoveSuccess:
                    return "200: Server: Tag removed from node(s).";
                case Responses.tagCreateSuccess:
                    return "200: Server: Tag created in tag bank.";
                case Responses.tagDeleteSuccess:
                    return "200: Server: Tag removed from tag bank.";
                case Responses.tagGetSuccess:
                    return "200: Server: Tag(s) retrieved.";
                case Responses.tagNotFound:
                    return "404: Database: Tag not found.";
                case Responses.tagDuplicate:
                    return "409: Database: The tag already exists.";
                case Responses.tagRetrievalFail:
                    return "502: Server: Unable to get tag data.";
                case Responses.tagCountInvalid:
                    return "422: Server: Invalid tag count.";
                case Responses.tagNameInvalid:
                    return "422: Server: Invalid tag name.";


                case Responses.userRateSuccess:
                    return "200: Server: User rating added.";
                case Responses.getRateSuccess:
                    return "200: Server: Rating retrieved.";
                case Responses.userRateFail:
                    return "502: Server: Unable rate node.";
                case Responses.getRateFail:
                    return "502: Server: Unable to get rating data.";


                case Responses.createNodeSuccess:
                    return "200: Server: Create Node Success";

                case Responses.jwtValidationSuccess:
                    return "200: Server: JWT Validation Success.";
                case Responses.jwtValidationFail:
                    return "400: Data: JWT Validation Failed: ";

                case Responses.refreshSessionSuccess:
                    return "200: Server: Refresh Session Success.";
                case Responses.refreshSessionFail:
                    return "400: Data: Refresh Session Failed: ";
                case Responses.refreshSessionNotAllowed:
                    return "400: Server: Refresh Session Failed: User is not logged in.";

                case Responses.accountUpdateSuccess:
                    return "200: Server: Account updated.";
                case Responses.accountEnableSuccess:
                    return "200: Server: Account enabled.";
                case Responses.accountDisableSuccess:
                    return "200: Server: Account disabled.";
                case Responses.accountUpdateFail:
                    return "502: Server: Unable to update account.";

                case Responses.deleteNodeSuccess:
                    return "200: Server: Delete Node Success";
                case Responses.getNodesSuccess:
                    return "200: Server: Get Nodes Success";
                case Responses.storeLogFail:
                    return "503: Database: Failed to store the log.";
                case Responses.badNameOrPass:
                    return "400: Data: Invalid Username or Passphrase. Please try again.";
                case Responses.badEmail:
                    return "400: Data: Invalid Email. Please try again.";
                case Responses.notAuthenticated:
                    return "401: Server: No active session found. Please login and try again.";
                case Responses.alreadyAuthenticated:
                    return "403: Server: Active session found. Please logout and try again.";
                case Responses.notAuthorized:
                    return "403: Database: You are not authorized to perform this operation.";
                case Responses.alreadyEnabled:
                    return "403: Server: UserAccount is already enabled.";
                case Responses.accountAlreadyConfirmed:
                    return "403: Server: UserAccount is already confirmed";
                case Responses.accountAlreadyUnconfirmed:
                    return "403: Server: UserAccount is already unconfirmed";
                case Responses.recoveryLinkLimitReached:
                    return "403: Server: UserAccount has reached limit of five attempts this month";
                case Responses.notFoundOrEnabled:
                    return "404: Database: The account was not found or it has been disabled.";
                case Responses.confirmationLinkNotFound:
                    return "404: Database: The confirmation link was not found.";
                case Responses.notFoundOrAuthorized:
                    return "404: Database: UserAccount not found or not authorized to perform the " +
                "operation.";
                case Responses.nodeNotFound:
                    return "504: Database: The node was not found.";
                case Responses.tagDoesNotExist:
                    return "404: Database: Tag not found.";
                case Responses.recoveryLinkNotFound:
                    return "404: Database: The recovery link was not found";
                case Responses.cancellationRequested:
                    return "408: Server: Cancellation token requested cancellation.";
                case Responses.accountAlreadyCreated:
                    return "409: Server: UserAccount  already exists";
                case Responses.recoveryLinkExists:
                    return "409: Database: The recovery link arealdy exists.";
                case Responses.confirmationLinkExists:
                    return "409: Database: The confirmation link already exists.";

                case Responses.recoveryLinkExpired:
                    return "410: Server: The recovery link has expired.";
                case Responses.confirmationLinkExpired:
                    return "410: Server: The confirmation link has expired.";

                case Responses.cookieFail:
                    return "503: Server: Authentication Cookie creation failed.";
                case Responses.sendEmailFail:
                    return "503: Server: Email failed to send.";
                case Responses.accountConfirmedFail:
                    return "503: Database: Failed to confirm the account";
                case Responses.accountUnconfirmedFail:
                    return "503: Database: Failed to unconfirm the account";
                case Responses.accountDisableFail:
                    return "503: Database: Failed to disable the account.";
                case Responses.accountEnableFail:
                    return "503: Database: Failed to enable the account.";
                case Responses.recoveryLinkRemoveFail:
                    return "503: Database: Failed to remove recovery link.";
                case Responses.confirmationLinkRemoveFail:
                    return "503: Database: Failed to remove confirmation link.";
                case Responses.confirmationLinkCreateFail:
                    return "504: Database: Failed to create confirmation linik.";
                case Responses.recoveryLinkCreateFail:
                    return "504: Database: Failed to create recovery link.";
                case Responses.otpFail:
                    return "503: Database: Failed to create OTP.";
                case Responses.databaseFail:
                    return "503: Database: The database is down. Please try again later.";
                case Responses.rollbackFailed:
                    return "504: Server rollback failed";
                case Responses.createNodeFail:
                    return "503: Database: Failed to create node.";
                case Responses.deleteNodeFail:
                    return "503: Database: Failed to delete node.";
                case Responses.updateNodeFail:
                    return "503: Database: Failed to update node.";
                case Responses.nodeAlreadyExists:
                    return "409: Database: Node Already Exists";
                case Responses.createdNodesExists:
                    return "Fail - Created Nodes Already Exists";
                case Responses.createdNodeNotExist:
                    return "Fail - Created Nodes to Update Does Not Exist in Database";
                case Responses.createdNodeNotInserted:
                    return "Fail - Created Nodes Not Inserted";
                case Responses.dailyLoginsExists:
                    return "Fail - Daily Logins Already Exists";
                case Responses.dailyLoginNotInserted:
                    return "Fail - Daily Logins Not Inserted";
                case Responses.dailyLoginNotExist:
                    return "Fail - Daily Logins to Update Does Not Exist in Database";
                case Responses.topSearchExists:
                    return "Fail - Top Search Already Exists";
                case Responses.topSearchNotInserted:
                    return "Fail - Top Search Not Inserted";
                case Responses.topSearchNotExist:
                    return "Fail - Top Search to Update Does Not Exist";
                case Responses.dailyRegistrationExists:
                    return "Fail - Daily Registration Already Exists";
                case Responses.dailyRegistrationNotInserted:
                    return "Fail - Daily Registration Not Inserted";
                case Responses.dailyRegistrationNotExist:
                    return "Fail - Daily Registration to Update Does Not Exist";



                case Responses.accountDeletionSuccess:
                    return "200: Server: UserAccount Deletion Successful.";
                case Responses.getAdminsSuccess:
                    return "200: Server: Get Admins Success.";
                case Responses.lastAdminFail:
                    return "503: Database: Cannot delete last admin in system";
                //this is when user clicks accountDeletion in twice before waiting for their first delete request to go thru
                case Responses.accountAlreadyDeleted:
                    return "503: Database: UserAccount is already deleted";
                case Responses.accountDeleteFail:
                    return "503: Database: Failed to delete account";
                case Responses.verificationFailure:
                    return "401: Server: Failure to authenticate and verify user for deletion.";



                default:
                    return "error";
            }
        }

        private Dictionary<string, string> InitializeErrorMessages()
        {
            Dictionary<string, string> errorMessages = new Dictionary<string, string>();
            // 400 Errors - Bad Request (Input, URL syntax)
            errorMessages.Add("badNameOrPass", "400: Data: Invalid Username or Passphrase. Please try again.");
            errorMessages.Add("badNameOrOTP", "400: Data: Invalid Username or OTP. Please try again.");
            errorMessages.Add("badEmail", "400: Data: Invalid Email. Please try again.");
            errorMessages.Add("tooManyFails", "400: Database: Too many fails have occurred. The account has been disabled.");
            errorMessages.Add("otpExpired", "400: Data: The OTP has expired. Please request a new one.");
            // 401 Errors - Unauthorized (Not authenticated)
            errorMessages.Add("notAuthenticated", "401: Server: No active session found. Please login and try again.");
            errorMessages.Add("notConfirmed", "401: Database: Please confirm your account before attempting to login.");
            // 403 Errors - Forbidden (User identity known, not Authorized)
            errorMessages.Add("alreadyAuthenticated", "403: Server: Active session found. Please logout and try again.");
            errorMessages.Add("notAuthorized", "403: Database: You are not authorized to perform this operation.");
            errorMessages.Add("alreadyEnabled", "403: Server: UserAccount is already enabled.");
            errorMessages.Add("recoveryLinkLimitReached", "403: Server: UserAccount has reached limit of five attempts this month");
            // 404 Errors - Not Found (Resource not found, interchangeable with 403 to hide existence
            //              of resource)
            errorMessages.Add("accountNotFound", "404: Database: The account was not found.");
            errorMessages.Add("notFoundOrEnabled", "404: Database: The account was not found or it has been disabled.");
            errorMessages.Add("notFoundOrAuthorized", "404: Database: UserAccount not found or not authorized to perform the " +
                "operation.");
            errorMessages.Add("nodeNotFound", "404: Database: The node was not found.");
            errorMessages.Add("recoveryLinkNotFound", "404: Database: The recovery link was not found");
            // 408 Errors - Server side timeout
            errorMessages.Add("cancellationRequested", "408: Server: Cancellation token requested cancellation.");


            //503 Errors - Service Unavailable (Server unable to handle request)
            errorMessages.Add("cookieFail", "503: Server: Authentication Cookie creation failed.");
            errorMessages.Add("sendEmailFail", "503: Server: Email failed to send.");
            errorMessages.Add("accountDisableFail", "503: Database: Failed to disable the account.");
            errorMessages.Add("accountEnableFail", "503: Database: Failed to enable the account.");
            errorMessages.Add("recoveryLinkRemoveFail", "503: Database: Failed to remove recovery link.");
            errorMessages.Add("recoveryLinkCreateFail", "504: Database: Failed to create recovery link.");
            errorMessages.Add("createNodeFail", "503: Database: Failed to create node.");
            errorMessages.Add("otpFail", "503: Database: Failed to create OTP.");
            errorMessages.Add("databaseFail", "503: Database: The database is down. Please try again later.");
            errorMessages.Add("logoutFail", "503: Server: Logout failed.");
            errorMessages.Add("rollbackFailed", "504: Server rollback failed");




            errorMessages.Add("createdNodesExists", "Fail - Created Nodes Already Exists");
            errorMessages.Add("createdNodeNotExist", "Fail - Created Nodes to Update Does Not Exist in Database");
            errorMessages.Add("createdNodeNotInserted", "Fail - Created Nodes Not Inserted");

            errorMessages.Add("dailyLoginsExists", "Fail - Daily Logins Already Exists");
            errorMessages.Add("dailyLoginNotInserted", "Fail - Daily Logins Not Inserted");
            errorMessages.Add("dailyLoginNotExist", "Fail - Daily Logins to Update Does Not Exist in Database");

            errorMessages.Add("topSearchExists", "Fail - Top Search Already Exists");
            errorMessages.Add("topSearchNotInserted", "Fail - Top Search Not Inserted");
            errorMessages.Add("topSearchNotExist", "Fail - Top Search to Update Does Not Exist");

            errorMessages.Add("dailyRegistrationExists", "Fail - Daily Registration Already Exists");
            errorMessages.Add("dailyRegistrationNotInserted", "Fail - Daily Registration Not Inserted");
            errorMessages.Add("dailyRegistrationNotExist", "Fail - Daily Registration to Update Does Not Exist");

            return errorMessages;
        }
    }
}
