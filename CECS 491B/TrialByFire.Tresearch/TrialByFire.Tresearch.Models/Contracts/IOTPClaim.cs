using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IOTPClaim
    {
        string Username { get; set; }

        string AuthorizationLevel { get; set; }
        string OTP { get; }
        DateTime TimeCreated { get; }

        int FailCount { get; set; }

        public string GenerateRandomOTP();
    }
}
