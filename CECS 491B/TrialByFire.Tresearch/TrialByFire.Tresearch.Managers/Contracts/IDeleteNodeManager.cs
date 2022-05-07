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
        Task<string> DeleteNodeAsync(IAccount account, long nodeID, long parentID, CancellationToken cancellation = default);
    }
}
