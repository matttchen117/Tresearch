using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationControllerShould
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public IMessageBank _messageBank { get; set; }

        public IRegistrationService _registrationService { get; set; }

        public IMailService _mailService { get; set; }

        public IValidationService _validationService { get; set; }
        public IRegistrationManager _registrationManager { get; set; }

        public IRegistrationController _registrationController { get; set; }

        public RegistrationControllerShould()
        {
            _messageBank = new MessageBank();
            _sqlDAO = new SqlDAO(_messageBank);
            _mailService = new MailService(_messageBank);
            _validationService = new ValidationService(_messageBank);
            _logService = new SqlLogService(_sqlDAO);
            _registrationService = new RegistrationService(_sqlDAO, _logService);
            _registrationManager = new RegistrationManager(_sqlDAO, _logService, _registrationService, _mailService, _validationService, _messageBank);
            _registrationController = new RegistrationController(_sqlDAO, _logService, _registrationService, _mailService, _messageBank, _validationService, _registrationManager);
        }

        [Theory]
        [InlineData("fezAshtray@gmail.com", "myRegisterPassword")]
        [InlineData("cassieKat@hotmail.com", "unFortunateName")]
        public void RegisterTheUser(string email, string passphrase)
        {
            // Arrange


            //Act
            string results = _registrationController.RegisterAccount(email, passphrase);

            //Assert
            Assert.Equal('S', results[0]);
        }

        public void ConfirmTheUser(string url)
        {

        }

    }
}
