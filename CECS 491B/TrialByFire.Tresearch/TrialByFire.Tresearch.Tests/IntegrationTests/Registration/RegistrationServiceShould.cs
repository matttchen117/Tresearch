using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationServiceShould : IntegrationTestDependencies
    {

        public IRegistrationService _registrationService { get; set; }

        public RegistrationServiceShould() : base()
        {
            _registrationService = new RegistrationService(SqlDAO, SqlLogService);
        }


        [Theory]
        [InlineData("wonderbread@gmail.com", "myRegisterPassword")]
        [InlineData("orowheat@hotmail.com", "unFortunateName")]
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange 
            IAccount account = new Account(email, email, passphrase, "user", true, false);

            //Act
            List<string> results = _registrationService.CreatePreConfirmedAccount(account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }


        [Theory]
        [InlineData("wheatIsGreat@gmail.com", "wheatIsGreat@gmail.com", "myPassword", "user", true, false, "www.Tresearch.systems/Registration/confirm?=")]
        [InlineData("whitebread@hotmail.com", "whitebread@hotmail.com", "servicePassword", "user", true, false, "www.Tresearch.systems/Registration/confirm?=")]
        public void CreateTheLink(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed, string baseUrl)
        {
            //Arrange
            IAccount _account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);

            //Act
            List<string> results = _registrationService.CreateConfirmation(email, baseUrl);

            //Assert
            Assert.Equal('S', results.Last()[0]);        // GUID contains 36 characters
        }

        [Theory]
        [InlineData("confirmMe@gmail.com", "myPassword", "User", true, false)]
        [InlineData("confirmMe2@gmail.com", "myPassword", "User", true, false)]
        public void ConfirmTheUser(string email, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            SqlDAO.CreateConfirmationLink(new ConfirmationLink(email, Guid.NewGuid(), DateTime.Now));

            IAccount _account = new Account(email, email, passphrase, authenticationLevel, status, confirmed);
            SqlDAO.CreateAccount(_account);
            //Act
            List<string> results = _registrationService.ConfirmAccount(_account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("getMyLink@gmail.com")]
        [InlineData("getMyLink2@gmail.com")]
        public void GetConfirmationLink(string email)
        {
            //Arrange
            Guid myguid = Guid.NewGuid();
            string guid = myguid.ToString();
            SqlDAO.CreateConfirmationLink(new ConfirmationLink(email, myguid, DateTime.Now));

            //Act
            IConfirmationLink link = _registrationService.GetConfirmationLink(guid);

            //Assert
            Assert.Equal(email, link.Username);
        }

        [Theory]
        [InlineData("removeMe@gmail.com")]
        [InlineData("removeMe2@gmail.com")]
        public void RemoveConfirmationLink(string email)
        {
            //Arrange
            IConfirmationLink link = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Now);
            SqlDAO.CreateConfirmationLink(link);

            //Act
            List<string> results = _registrationService.RemoveConfirmationLink(link);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
    }
}
