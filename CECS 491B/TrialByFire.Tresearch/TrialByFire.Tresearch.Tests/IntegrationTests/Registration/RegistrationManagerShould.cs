using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationManagerShould
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public IMessageBank _messageBank { get; set; }

        public IRegistrationService _registrationService { get; set; }

        public IMailService _mailService { get; set; }

        public IValidationService _validationService { get; set; }
        public IRegistrationManager _registrationManager { get; set; }

        public RegistrationManagerShould()
        {
            _messageBank = new MessageBank();
            _sqlDAO = new SqlDAO(_messageBank);
            _mailService = new MailService(_messageBank);
            _validationService = new ValidationService(_messageBank);
            _logService = new SqlLogService(_sqlDAO);
            _registrationService = new RegistrationService(_sqlDAO, _logService);
            _registrationManager = new RegistrationManager(_sqlDAO, _logService, _registrationService, _mailService, _validationService, _messageBank);
        }
        [Theory]
        [InlineData("skiPatrol@gmail.com", "myRegisterPassword")]
        [InlineData("snowboarder@hotmail.com", "unFortunateName")]
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange 

            //Act
            List<string> results = _registrationManager.CreatePreConfirmedAccount(email, passphrase);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("confirmMeMan@gmail.com", "myPassword", "user", true, false)]
        [InlineData("confirmMeMan2@gmail.com", "myPassword", "user", true, false)]
        public void ConfirmTheUser(string email, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            IConfirmationLink link = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Now);
            string url = link.UniqueIdentifier.ToString();
            _sqlDAO.CreateConfirmationLink(link);
            IAccount _account = new Account(email, email, passphrase, authenticationLevel, status, confirmed);
            _sqlDAO.CreateAccount(_account);

            //Act
            List<string> results = _registrationManager.ConfirmAccount(url);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com")]
        [InlineData("jelazo@live.com", "www.tresearch.systems/")]
        public void SendConfirmation(string email, string url)
        {

            //Act
            List<string> results = _registrationManager.SendConfirmation(email, url);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }


        [Theory]
        [InlineData("cottonEyedJoe@gmail.com", -1)]
        [InlineData("innnout@live.com", 0)]
        public void checkLinkValidity(string email, int sub)
        {
            //Arrange
            DateTime now = DateTime.Now;
            IConfirmationLink link = new ConfirmationLink(email, Guid.NewGuid(), now.AddDays(sub));
            Boolean expected;
            if (sub < 0)
                expected = false;
            else
                expected = true;

            //Act
            bool actual = _registrationManager.IsConfirmationLinkValid(link);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
