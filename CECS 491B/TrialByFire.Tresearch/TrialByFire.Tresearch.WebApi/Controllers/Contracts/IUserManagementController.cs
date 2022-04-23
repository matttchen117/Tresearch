using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUserManagementController
    {
        public Task<IActionResult> CreateAccountAsync(string username, string passphrase, string authorizationLevel);
        public Task<IActionResult> UpdateAccountAsync(List<IAccount> accounts);
        public Task<IActionResult> DeleteAccountAsync(IAccount account);
        public Task<IActionResult> EnableAccountAsync(IAccount account);
        public Task<IActionResult> DisableAccountAsync(IAccount account);
    }
}
