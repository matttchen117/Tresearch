using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ILogoutController
    {
        public Task<IActionResult> Logout();
    }
}
