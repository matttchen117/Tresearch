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
using Microsoft.AspNetCore.Mvc;
using Xunit;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationControllerShould : IntegrationTestDependencies
    {
        public IRegistrationService _registrationService { get; set; }

        public IMailService _mailService { get; set; }
        public IRegistrationManager _registrationManager { get; set; }

        public IRegistrationController _registrationController { get; set; }

        public RegistrationControllerShould() : base()
        {
            _mailService = new MailService(MessageBank);
            _registrationService = new RegistrationService(SqlDAO, SqlLogService);
            _registrationManager = new RegistrationManager(SqlDAO, SqlLogService, _registrationService, _mailService, ValidationService, MessageBank);
            _registrationController = new RegistrationController(SqlDAO, SqlLogService, _registrationService, _mailService, MessageBank, ValidationService, _registrationManager);
        }

        [Theory]
        [InlineData("fezAshtray@gmail.com", "myRegisterPassword")]
        [InlineData("cassieKat@hotmail.com", "unFortunateName")]
        public void RegisterTheUser(string email, string passphrase)
        {
            // Arrange


            //Act
            IActionResult results = _registrationController.RegisterAccount(email, passphrase);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }

        public void ConfirmTheUser(string url)
        {

        }

    }
}
