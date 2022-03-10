using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

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
            successMessages.Add("generic", "success");
            return successMessages;
        }
        private Dictionary<string, string> InitializeErrorMessages()
        {
            Dictionary<string, string> errorMessages = new Dictionary<string, string>();
            errorMessages.Add("badNameOrPass", "Data: Invalid Username or Passphrase. Please try again.");
            errorMessages.Add("badNameOrOTP", "Data: Invalid Username or OTP. Please try again.");
            errorMessages.Add("badEmail", "Data: Invalid Email. Please try again.");
<<<<<<< HEAD
            errorMessages.Add("notAuthorized", "Database: You are not authorized to perform this operation.");
            errorMessages.Add("notAuthenticated", "Server: No active session found. Please login and try again.");
            errorMessages.Add("alreadyAuthenticated", "Server: Active session found. Please logout and try again.");
            errorMessages.Add("notConfirmed", "Database: Please confirm your account before attempting to login.");
            errorMessages.Add("accountNotFound", "Database: The account was not found.");
            errorMessages.Add("notFoundOrEnabled", "Database: The account was not found or it has been disabled.");
=======
>>>>>>> TestPammyMerge
            errorMessages.Add("otpExpired", "Data: The OTP has expired. Please request a new one.");

            errorMessages.Add("cookieFail", "Server: Authentication Cookie creation failed.");
            errorMessages.Add("sendEmailFail", "Server: Email failed to send.");
            errorMessages.Add("alreadyAuthenticated", "Server: Active session found. Please logout and try again.");
            
            errorMessages.Add("accountDisableFail", "Database: Failed to disable the account.");
            errorMessages.Add("notFoundOrAuthorized", "Database: Account not found or not authorized to perform the " +
                "operation.");
            errorMessages.Add("otpFail", "Database: Failed to create OTP.");
            errorMessages.Add("databaseFail", "Database: The database is down. Please try again later.");
<<<<<<< HEAD
            errorMessages.Add("logoutFail", "Server: Logout failed.");
=======
            errorMessages.Add("notAuthorized", "Database: You are not authorized to perform this operation.");
            errorMessages.Add("notAuthenticated", "Database: No active session found. Please login and try again.");
            errorMessages.Add("notConfirmed", "Database: Please confirm your account before attempting to login.");
            errorMessages.Add("accountNotFound", "Database: The account was not found.");
            errorMessages.Add("notFoundOrEnabled", "Database: The account was not found or it has been disabled.");
            errorMessages.Add("tooManyFails", "Database: Too many fails have occurred. The account has been disabled.");


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

>>>>>>> TestPammyMerge
            return errorMessages;
        }
    }
}
