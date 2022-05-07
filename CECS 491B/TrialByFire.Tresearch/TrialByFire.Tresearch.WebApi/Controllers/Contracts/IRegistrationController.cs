using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IRegistrationController
    {
        public Task<IActionResult> RegisterAccountAsync(string email, string passphrase);
        public Task<IActionResult> ConfirmAccountAsync(string guid);
        public Task<IActionResult> ResendConfirmationLinkAsync(string guid);
    }
}
