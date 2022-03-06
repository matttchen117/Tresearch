using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationControllerShould
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public IMailService _mailService { get; set; }
        public IRegistrationService _registrationService { get; set; }

        public IValidationService _validationService { get; set; }

        public IMessageBank _messageBank { get; set; }

        public IRegistrationManager _registrationManager { get; set; }
        public void RegisterTheUser(string _username, string passphrase)
        {
            // Arrange
            IMessageBank _messageBank = new MessageBank();
            ISqlDAO _sqlDAO = new SqlDAO(_messageBank);
            ILogService _logService = new SqlLogService(_sqlDAO);
            IRegistrationService _accountService = new RegistrationService(_sqlDAO, _logService);
            IMailService _mailService = new MailService(_messageBank);
            //IRegistrationManager _accountManager = new RegistrationManager(_sqlDAO, _logService, _accountService, _mailService, validationService, );

            //Act

            //_accountManager.CreatePreConfirmedAccount("pammypoor@gmail.com", "myPassword");

            //Assert
        }

        public void ConfirmTheUser(string url)
        {

        }

    }
}
