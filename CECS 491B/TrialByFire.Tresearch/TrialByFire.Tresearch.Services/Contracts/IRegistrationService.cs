using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;



namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRegistrationService
    {
        public Task<string> CreateAccountAsync(string email, string passphrase, string authorizationlevel, CancellationToken cancellation = default(CancellationToken));
        public Task<Tuple<IConfirmationLink, string>> CreateConfirmationAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> ConfirmAccountAsync(string username, string authenticationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveConfirmationLinkAsync(IConfirmationLink _confirmationLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateConfirmationAsync(IConfirmationLink link, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> HashValueAsync(string value, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateHashTableEntry(string email, string authorizationLevel, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken));
    }
}
