using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ICreateNodeManager
    {
        Task<string> CreateNodeAsync(string username, Node node, CancellationToken cancellationToken = default);
    }
}
