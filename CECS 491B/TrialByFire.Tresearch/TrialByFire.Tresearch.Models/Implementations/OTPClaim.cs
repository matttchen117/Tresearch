using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Exceptions;

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
            if ((username ?? otp) == null)
            {
                throw new OTPClaimCreationFailedException("Data: OTP Claim creation failed. Null argument passed in for" +
                    "username or otp.");
            }
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
            if((account) == null)
            {
                throw new OTPClaimCreationFailedException("Data: OTP Claim creation failed. Null argument passed in for" +
                    "account.");
            }
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

        public override bool Equals(object? obj)
        {
            if (obj != null)
            {
                if(obj is IOTPClaim claim)
                {
                    IOTPClaim dbOTPClaim = (IOTPClaim)obj;
                    return this.Username == dbOTPClaim.Username;
                }
            }
            return false;
        }
    }
}
