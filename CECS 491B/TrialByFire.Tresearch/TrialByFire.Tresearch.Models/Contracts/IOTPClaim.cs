using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IOTPClaim
    {
        string _username { get; }
        string _otp { get; }
        DateTime _created { get; }

        public string GenerateRandomOTP();
    }
}
