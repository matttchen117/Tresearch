using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUserManagementController
    {
        public Task<IActionResult> CreateAccountAsync(IAccount account);
        public Task<IActionResult> UpdateAccountAsync(IAccount account, IAccount updatedAccount);
        public Task<IActionResult> DeleteAccountAsync(IAccount account);
        public Task<IActionResult> EnableAccountAsync(IAccount account);
        public Task<IActionResult> DisableAccountAsync(IAccount account);
    }
}
