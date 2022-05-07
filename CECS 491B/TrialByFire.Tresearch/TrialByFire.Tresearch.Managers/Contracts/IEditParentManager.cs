using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IEditParentManager
    {
        public Task<IResponse<string>> EditParentNodeAsync(string userhash, long nodeID, string nodeIDs, CancellationToken cancellationToken = default);
    }
}
