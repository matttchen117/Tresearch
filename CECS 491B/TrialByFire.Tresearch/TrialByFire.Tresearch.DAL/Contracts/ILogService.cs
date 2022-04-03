using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    public interface ILogService
    {
        public Task<string> StoreLogAsync(DateTime timestamp, string level, string username, 
            string category, string description, CancellationToken cancellationToken = default);
    }
}
