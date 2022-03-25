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
        public IMailService _mailService { get; set; }
        public IRegistrationService _registrationService { get; set; }
        public IValidationService _validationService { get; set; }
        public Task<string> CreateAndSendConfirmationAsync(string email, string passphrases, string authorizationLevel, string baseUrl, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> ConfirmAccountAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public bool IsConfirmationLinkValid(IConfirmationLink confirmationLink);
    }
}
