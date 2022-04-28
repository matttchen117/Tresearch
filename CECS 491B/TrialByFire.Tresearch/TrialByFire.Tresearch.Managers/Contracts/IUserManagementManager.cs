using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IUserManagementManager
    {
        public Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken);
        public Task<string> EnableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DisableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }
}
