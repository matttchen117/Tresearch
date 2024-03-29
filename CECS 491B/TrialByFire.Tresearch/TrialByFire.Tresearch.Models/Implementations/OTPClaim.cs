﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        public string Username { get; set; }

        public string AuthorizationLevel { get; set; }

        public string OTP { get; }

        public DateTime TimeCreated { get; }

        public int FailCount { get; set; }

        public OTPClaim(string username, string otp, string authorizationLevel, DateTime timeCreated, int failCount)
        {
            if ((username ?? otp) == null)
            {
                throw new OTPClaimCreationFailedException("Data: OTP Claim creation failed. Null argument passed in for" +
                    "username or otp.");
            }
            Username = username;
            AuthorizationLevel = authorizationLevel;
            OTP = otp;
            TimeCreated = timeCreated;
            FailCount = failCount;
        }

        public OTPClaim(string username, string otp, string authorizationLevel, DateTime timeCreated)
        {
            if ((username ?? otp) == null)
            {
                throw new OTPClaimCreationFailedException("Data: OTP Claim creation failed. Null argument passed in for" +
                    "username or otp.");
            }
            Username = username;
            AuthorizationLevel = authorizationLevel;
            OTP = otp;
            TimeCreated = timeCreated;
            FailCount = 0;
        }

        public OTPClaim(IAccount account, string otp)
        {
            Username = account.Username;
            AuthorizationLevel = account.AuthorizationLevel;
            OTP = otp;
            TimeCreated = DateTime.Now.ToUniversalTime().ToUniversalTime();
            FailCount = 0;
        }

        public OTPClaim() { }
        

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is IOTPClaim)
                {
                    IOTPClaim otpClaim = (IOTPClaim)obj;
                    return Username.Equals(otpClaim.Username) && AuthorizationLevel.Equals(otpClaim.AuthorizationLevel);
                }
            }
            return false;
        }
    }
}
