using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface INodeSearchService
    {
        public Task<IResponse<IList<Node>>> SearchForNodeAsync(ISearchInput searchInput);
    }
}
