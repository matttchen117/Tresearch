using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    internal interface IOTPClaim
    {
        string username { get; }
        string otp { get; }
        DateTime created { get; }
    }
}
