using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ITreeManagementController
    {
        public Task<IActionResult> GetNodesAsync(string userHash);
    }
}