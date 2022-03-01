using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IOtpRequestManager
    {
        string RequestOTP(string username, string passphrase);
    }
}
