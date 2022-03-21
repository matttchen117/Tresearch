using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IAuthenticationController
    {
        public Task<IActionResult> AuthenticateAsync(string username, string otp, string authorizationLevel);
        public Task<IActionResult> AuthenticateAsync(string username, string otp, string authorizationLevel, DateTime now);
    }
}
