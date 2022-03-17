using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IOTPRequestManager
    {
        Task<string> RequestOTPAsync(string username, string passphrase, 
            string authorizationLevel, CancellationToken cancellationToken = default);
    }
}
