using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Implementations;
namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ICreateNodeController
    {
        public Task<ActionResult<string>> CreateNodeAsync(string userhash, long parentNode, string nodeTitle, string summary);
    }
}
