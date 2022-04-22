using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface INodeSearchController
    {
        public Task<ActionResult<IEnumerable<Node>>> SearchForNodeAsync(string search, IEnumerable<string> tags, bool filterByRating,
            bool filterByTime);
    }
}
