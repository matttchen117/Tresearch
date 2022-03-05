using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class OTPClaim : IOTPClaim
    {
        public string Username { get; }

        public string OTP { get; }

        public DateTime TimeCreated { get; }

        public int FailCount { get; set; }

        public OTPClaim(string username, string otp, DateTime timeCreated)
        {
            Username = username;
            OTP = otp;
            TimeCreated = timeCreated;
            FailCount = 0;
        }

        public OTPClaim(string username, string otp, DateTime timeCreated, int failCount)
        {
            if ((username ?? otp) == null)
            {
                throw new OTPClaimCreationFailedException("Data: OTP Claim creation failed. Null argument passed in for" +
                    "username or otp.");
            }
            Username = username;
            OTP = otp;
            TimeCreated = timeCreated;
            FailCount = failCount;
        }

        public OTPClaim(IAccount account)
        {
            Username = account.Username;
            OTP = GenerateRandomOTP();
            TimeCreated = DateTime.Now;
            FailCount = 0;
        }

        public string GenerateRandomOTP()
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            int length = random.Next(8, 17);
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += validCharacters[random.Next(0, validCharacters.Length)];
            }
            return otp;
        }
    }
}
