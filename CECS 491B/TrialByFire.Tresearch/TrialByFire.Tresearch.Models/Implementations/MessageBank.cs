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
            errorMessages.Add("notAuthorized", "Database: You are not authorized to perform this operation.");
            errorMessages.Add("notAuthenticated", "Database: No active session found. Please login and try again.");
            errorMessages.Add("alreadyAuthenticated", "Server: Active session found. Please logout and try again.");
            errorMessages.Add("notConfirmed", "Database: Please confirm your account before attempting to login.");
            errorMessages.Add("notFoundOrEnabled", "Database: The account was not found or it has been disabled.");
            errorMessages.Add("otpExpired", "Data: The OTP has expired. Please request a new one.");
            errorMessages.Add("tooManyFails", "Database: Too many fails have occurred. The account has been disabled.");
            errorMessages.Add("cookieFail", "Server: Authentication Cookie creation failed");
            return errorMessages;
        }
    }
}
