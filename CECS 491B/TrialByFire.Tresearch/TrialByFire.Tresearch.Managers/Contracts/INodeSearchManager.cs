using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface INodeSearchManager
    {
        public Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput);
    }
}
