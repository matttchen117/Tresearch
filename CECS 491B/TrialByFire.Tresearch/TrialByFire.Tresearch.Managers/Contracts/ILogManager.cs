using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ILogManager
    {
        public Task<string> StoreAnalyticLogAsync(DateTime timestamp, string level, string username,
            string authorizationLevel, string category, string description,
            CancellationToken cancellationToken = default);
        public Task<string> StoreArchiveLogAsync(DateTime timestamp, string level, string username,
            string authorizationLevel, string category, string description,
            CancellationToken cancellationToken = default);
    }
}
