using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IAuthenticationController
    {
        public Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, CancellationToken cancellationToken = default);
        public Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, DateTime now, CancellationToken cancellationToken = default);
    }
}
