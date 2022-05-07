using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IDeleteNodeService
    {
        Task<string> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default);
    }
}
