using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IDeleteNodeController
    {
        public Task<IActionResult> DeleteNodeAsync(IAccount account, long nodeID, long parentID);
    }
}
