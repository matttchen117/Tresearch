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
                case Responses.storeLogFail:
                    return "503: Database: Failed to store the log.";
                case Responses.badNameOrPass:
                    return "400: Data: Invalid Username or Passphrase. Please try again.";
                case Responses.badNameOrOTP:
                    return "400: Data: Invalid Username or OTP. Please try again.";
                case Responses.badEmail:
                    return "400: Data: Invalid Email. Please try again.";
                case Responses.tooManyFails:
                    return "400: Database: Too many fails have occurred. The account has been disabled.";
                case Responses.otpExpired:
                    return "400: Data: The OTP has expired. Please request a new one.";
                case Responses.notAuthenticated:
                    return "401: Server: No active session found. Please login and try again.";
                case Responses.notConfirmed:
                    return "401: Database: Please confirm your account before attempting to login.";
                case Responses.alreadyAuthenticated:
                    return "403: Server: Active session found. Please logout and try again.";
                case Responses.notAuthorized:
                    return "403: Database: You are not authorized to perform this operation.";
                case Responses.alreadyEnabled:
                    return "403: Server: Account is already enabled.";
                case Responses.recoveryLinkLimitReached:
                    return "403: Server: Account has reached limit of five attempts this month";
                case Responses.accountNotFound:
                    return "404: Database: The account was not found.";
                case Responses.notFoundOrEnabled:
                    return "404: Database: The account was not found or it has been disabled.";
                case Responses.notFoundOrAuthorized:
                    return "404: Database: Account not found or not authorized to perform the " +
                "operation.";
                case Responses.recoveryLinkNotFound:
                    return "404: Database: The recovery link was not found";
                case Responses.cancellationRequested:
                    return "408: Server: Cancellation token requested cancellation.";
                case Responses.recoveryLinkExists:
                    return "409: Database: The recovery link arealdy exists.";
                case Responses.recoveryLinkExpired:
                    return "410: Server: The recovery link has expired.";
                case Responses.cookieFail:
                    return "503: Server: Authentication Cookie creation failed.";
                case Responses.sendEmailFail:
                    return "503: Server: Email failed to send.";
                case Responses.accountDisableFail:
                    return "503: Database: Failed to disable the account.";
                case Responses.accountEnableFail:
                    return "503: Database: Failed to enable the account.";
                case Responses.recoveryLinkRemoveFail:
                    return "503: Database: Failed to remove recovery link.";
                case Responses.recoveryLinkCreateFail:
                    return "504: Database: Failed to create recovery link.";
                case Responses.otpFail:
                    return "503: Database: Failed to create OTP.";
                case Responses.databaseFail:
                    return "503: Database: The database is down. Please try again later.";
                case Responses.logoutFail:
                    return "503: Server: Logout failed.";
                case Responses.rollbackFailed:
                    return "504: Server rollback failed";
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
            errorMessages.Add("alreadyEnabled", "403: Server: Account is already enabled.");
            errorMessages.Add("recoveryLinkLimitReached", "403: Server: Account has reached limit of five attempts this month");
            // 404 Errors - Not Found (Resource not found, interchangeable with 403 to hide existence
            //              of resource)
            errorMessages.Add("accountNotFound", "404: Database: The account was not found.");
            errorMessages.Add("notFoundOrEnabled", "404: Database: The account was not found or it has been disabled.");
            errorMessages.Add("notFoundOrAuthorized", "404: Database: Account not found or not authorized to perform the " +
                "operation.");
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
