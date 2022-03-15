using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IOTPRequestController
    {
        public Task<IActionResult> RequestOTPAsync(string username, string passphrase, string authorizationLevel);
    }
}
