using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;


namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IRegistrationManager
    {
        public Task<string> CreateAndSendConfirmationAsync(string email, string passphrases, string authorizationLevel, CancellationToken cancellationToken = default);
        public Task<string> ConfirmAccountAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public bool IsConfirmationLinkInvalid(IConfirmationLink confirmationLink);
        public Task<string> ResendConfirmation(string guid, CancellationToken cancellationToken = default(CancellationToken));
    }
}
