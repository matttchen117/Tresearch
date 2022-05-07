using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ICreateNodeManager
    {
        Task<IResponse<string>> CreateNodeAsync(string userhash, Node node, CancellationToken cancellationToken = default);
    }
}
