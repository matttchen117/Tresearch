using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IRegistrationController
    {
<<<<<<< HEAD
        public Task<IActionResult> RegisterAccountAsync(string email, string passphrase);
        public Task<IActionResult> ConfirmAccountAsync(string guid);
        public Task<IActionResult> ResendConfirmationLinkAsync(string guid);
=======
        [HttpPost("register")]
        public string RegisterAccount(string email, string passphrase);

        public string SendConfirmation(string email);

        public string ConfirmAccount(string url);
>>>>>>> Working
    }
}
