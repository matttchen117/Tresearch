using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IRegistrationController
    {
        [HttpPost("register")]
        public Task<IActionResult> RegisterAccountAsync(string email, string passphrase);
        public Task<IActionResult> ConfirmAccountAsync(string guid);
        public Task<IActionResult> ResendConfirmationLink(string guid);
    }
}
