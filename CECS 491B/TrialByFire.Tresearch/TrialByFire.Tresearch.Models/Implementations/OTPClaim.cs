using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class OTPClaim : IOTPClaim
    {
        public string username { get; }

        public string otp { get; }

        public DateTime created { get; }

        public OTPClaim(string username, string otp, DateTime created)
        {
            this.username = username;
            this.otp = otp;
            this.created = created;
        }

    }
}
