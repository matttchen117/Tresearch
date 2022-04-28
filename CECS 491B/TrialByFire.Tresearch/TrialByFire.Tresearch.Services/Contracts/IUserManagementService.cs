using TrialByFire.Tresearch.Models.Contracts;
namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IUserManagementService
    {
        public Task<Tuple<IConfirmationLink, string>> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> EnableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DisableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }
}
