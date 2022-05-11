using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface INodeContentController
    {
        public Task<IActionResult> UpdateNodeContentAsync(string owner, long nodeID, string title, string summary);
    }
}
