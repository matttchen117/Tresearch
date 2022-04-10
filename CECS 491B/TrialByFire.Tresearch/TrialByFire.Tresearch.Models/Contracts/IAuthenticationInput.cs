using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IAuthenticationInput
    {
        public IAccount? Account { get; set; }
        public IOTPClaim? OTPClaim { get; set; }

        string? UserHash { get; set; }
    }
}
