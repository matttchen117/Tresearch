using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ITreeManagementManager
    {
        Task<Tuple<Tree, string>> GetNodesAsync(string userHash, CancellationToken cancellationToken = default);
    }
}
