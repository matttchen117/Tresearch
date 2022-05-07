using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ICreateNodeService
    {
        Task<IResponse<string>> CreateNodeAsync(INode node, CancellationToken cancellationToken = default);
    }
}
