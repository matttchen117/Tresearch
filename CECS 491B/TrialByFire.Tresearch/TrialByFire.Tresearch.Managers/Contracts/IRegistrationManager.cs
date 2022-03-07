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

        public List<string> SendConfirmation(string email, string baseUrl);

        public List<string> CreatePreConfirmedAccount(string email, string passphrase);

        public List<string> ConfirmAccount(string url);

        public bool IsConfirmationLinkValid(IConfirmationLink confirmationLink);


    }
}
