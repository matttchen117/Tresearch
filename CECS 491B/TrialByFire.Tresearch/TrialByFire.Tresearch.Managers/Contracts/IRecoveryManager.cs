using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IRecoveryManager
    {
        public Task<string> SendRecoveryEmail(string email, string baseurl, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> EnableAccountAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
    }
}
