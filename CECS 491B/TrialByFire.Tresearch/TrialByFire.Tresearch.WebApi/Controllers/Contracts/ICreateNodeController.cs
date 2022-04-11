using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Implementations;
namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ICreateNodeController
    {
        public Task<IActionResult> CreateNodeAsync(string username, Node node);
    }
}
