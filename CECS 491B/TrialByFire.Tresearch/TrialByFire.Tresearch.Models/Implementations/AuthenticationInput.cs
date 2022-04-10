using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class AuthenticationInput : IAuthenticationInput
    {
        public IAccount? UserAccount { get; set; }
        public IOTPClaim? OTPClaim { get; set; }
        public string? UserHash { get; set; }
        public AuthenticationInput(IAccount account)
        {
            UserAccount = account;
        }

        public AuthenticationInput(IAccount account, IOTPClaim otpClaim)
        {
            UserAccount = account;
            OTPClaim = otpClaim;
        }

        public AuthenticationInput(IAccount account, string userHash)
        {
            UserAccount = account;
            UserHash = userHash;
        }

        public AuthenticationInput(IOTPClaim otpClaim, string userHash)
        {
            OTPClaim = otpClaim;
            UserHash = userHash;
        }

        public AuthenticationInput(IAccount account, IOTPClaim otpClaim, string userHash)
        {
            UserAccount = account;
            OTPClaim = otpClaim;
            UserHash = userHash;
        }
    }
}
