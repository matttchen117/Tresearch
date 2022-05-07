using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IDeleteNodeManager
    {
        Task<IResponse<string>> DeleteNodeAsync(string userhash, long nodeID, long parentID, CancellationToken cancellationToken = default);
    }
}
