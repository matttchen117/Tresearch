using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;

namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{

    public class InMemoryRegistrationControllerShould
    {
        ISqlDAO _sqlDAO { get; set; }
        ILogService _logService { get; set; }

        IRegistrationService _registrationService { get; set; }

        IMailService _mailService { get; set; }

        IValidationService _validationService { get; set; }

        IMessageBank _messageBank { get; set; }

        IRegistrationManager _registrationManager { get; set; }

        IRegistrationController _registrationController { get; set; }

        public InMemoryRegistrationControllerShould()
        {
            _sqlDAO = new InMemorySqlDAO();
            _logService = new InMemoryLogService(_sqlDAO);
            _registrationService = new RegistrationService(_sqlDAO, _logService);
            
            _messageBank = new MessageBank();
            _mailService = new MailService(_messageBank);
            _validationService = new ValidationService(_messageBank);
            _registrationManager = new RegistrationManager(_sqlDAO, _logService, _registrationService, _mailService, _validationService, _messageBank);
            _registrationController = new RegistrationController(_sqlDAO, _logService, _registrationService, _mailService, _messageBank, _validationService, _registrationManager);
        }

        [Theory]
        [InlineData("pammypoor+INcontrollerRegister1@gmail.com", "myValidPassphrase")]
        [InlineData("pammypoor+INcontrollerRegister2@gmail.com", "ApplePie!")]
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange

            //Act
            string result = _registrationController.RegisterAccount(email, passphrase);

            //Assert
            Assert.Equal('S', result[0]);
        }



        [Theory]
        [InlineData("pammypoor+INcontrollerSendConfirmation1@gmail.com")]
        [InlineData("pammypoor+INcontrollerSendConfirmation2@gmail.com")]
        public void SendConfirmation(string email)
        {
            //Arrange
            IAccount account = new Account(email, "temporaryPassword");

            //Act
            string result = _registrationController.SendConfirmation(email);

            //Assert
            Assert.Equal('S', result[0]);
        }


        [Theory]
        [InlineData("pammypoor+INcontrollerConfirm1@gmail.com", "www.tresearch.systems/Registration/verify?=1CCB92FC-2859-E432-BCCA-1104FCA5DADA", "2022-03-05 21:32:59.910")]
        [InlineData("pammypoor+INcontrollerConfirm1@gmail.com", "www.tresearch.systems/Registration/verify?=1CCB02FC-A859-E432-AND1-1104FCA5DADA", "2022-03-05 21:32:59.910")]
        public void confirmAccount(string email, string url, string date)
        {
            //Arrange
            IConfirmationLink _confirmationLink = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Parse(date));
            _sqlDAO.CreateConfirmationLink(_confirmationLink);

            //Act
            string result = _registrationController.ConfirmAccount(url);


            //Assert
            Assert.Equal('S', result[0]);
        }
    }
}
