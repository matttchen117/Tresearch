﻿using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRecoveryService
    {
        
        public Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<Tuple<IAccount, string>>GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<Tuple<IRecoveryLink, string>> CreateRecoveryLinkAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
    }
}
