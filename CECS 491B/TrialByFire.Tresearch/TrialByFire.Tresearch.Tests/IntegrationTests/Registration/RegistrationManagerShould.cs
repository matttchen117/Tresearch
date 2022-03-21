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
    public class RegistrationManagerShould : IntegrationTestDependencies
    {

        public IRegistrationService _registrationService { get; set; }

        public IMailService _mailService { get; set; }
        public IRegistrationManager _registrationManager { get; set; }

        public RegistrationManagerShould() : base()
        {
            _mailService = new MailService(MessageBank);
            _registrationService = new RegistrationService(SqlDAO, LogService);
            _registrationManager = new RegistrationManager(SqlDAO, LogService, _registrationService, _mailService, ValidationService, MessageBank);
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
            SqlDAO.CreateConfirmationLink(link);
            IAccount _account = new Account(email, email, passphrase, authenticationLevel, status, confirmed);
            SqlDAO.CreateAccount(_account);

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
