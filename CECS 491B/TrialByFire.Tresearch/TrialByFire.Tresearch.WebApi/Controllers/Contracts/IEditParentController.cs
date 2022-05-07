using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IEditParentController
    {
        public Task<ActionResult<string>> EditParentNodeAsync(string userhash, long nodeID, string nodeIDs);
    }
}
