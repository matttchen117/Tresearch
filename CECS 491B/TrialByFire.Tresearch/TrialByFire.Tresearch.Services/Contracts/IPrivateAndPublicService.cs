using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IPrivateAndPublicService
    {
        public Task<IResponse<string>> PrivateNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken));

        //public Task<IResponse<string>> PublicNodeAsync();

    }
}
