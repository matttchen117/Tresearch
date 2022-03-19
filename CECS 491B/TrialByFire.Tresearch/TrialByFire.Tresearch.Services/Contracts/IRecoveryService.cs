using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRecoveryService
    {
        public Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(Guid guid, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> RemoveAllRecoveryLinksAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        public Task<Tuple<IAccount, string>>GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));

        public Task<Tuple<IRecoveryLink, string>> CreateRecoveryLinkAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        public Task<Tuple<bool, string>> IsAccountDisabledAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> EnableAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }
}
