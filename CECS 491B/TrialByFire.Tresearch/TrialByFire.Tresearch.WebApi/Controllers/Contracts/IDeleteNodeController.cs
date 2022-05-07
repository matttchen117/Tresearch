using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IDeleteNodeController
    {
        //public Task<ActionResult<string>> DeleteNodeAsync(string userhash, long nodeID, long parentID);
        public Task<ActionResult<string>> DeleteNodeAsync(Node node);
    }
}
