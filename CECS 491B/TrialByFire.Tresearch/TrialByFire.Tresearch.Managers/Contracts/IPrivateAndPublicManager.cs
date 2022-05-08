using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IPrivateAndPublicManager
    {
        public Task<IResponse<string>> PrivateNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<string>> PublicNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken));

    }
}
