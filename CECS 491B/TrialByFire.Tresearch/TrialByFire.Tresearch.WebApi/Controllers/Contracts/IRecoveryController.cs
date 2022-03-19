using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IRecoveryController
    {
        public Task<IActionResult> SendRecoveryEmailAsync(string email, string authorizationLevel);
    }
}
