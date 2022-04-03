using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationServiceShould
    {
        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor@gmail.com", "myPassphrase", "U", true, false)]
        [InlineData("ValasquezJerry@gmail.com", "ValasquezJerry@gmail.com", "oooooPPPPPP", "U", true, false)]
        public void ConfirmTheAccount(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            IAccount account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);
            _sqlDAO.CreateAccount(account);

            //Act
            List<string> results = _registrationService.ConfirmAccount(account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor@gmail.com", "superSecretPassphrase", "U", true, false)]
        [InlineData("JEdgarHoover@usa.gov", "JEdgarHoover@usa.gov", "helloHello123", "U", true, false)]
        public void CreateTheUser(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            IAccount account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);

            //Act
            List<string> results = _registrationService.CreatePreConfirmedAccount(account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }


        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor@gmail.com", "myPassphrase", "U", true, false, "www.Tresearch.systems/")]
        [InlineData("h.poor@cox.net", "h.poor@cox.net", "password123", "U", true, false, "www.Tresearch.systems/")]
        public void CreateTheLink(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed, string baseUrl)
        {
            //Arrange
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            IAccount account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);

            //Act
            List<string> results = _registrationService.CreateConfirmation(email, baseUrl);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("wilburt@gmail.com", "www.Tresearch.systems/")]
        [InlineData("maxwell123@hotmails=.com", "www.Tresearch.systems/")]
        public void GetTheLink(string username, string url)
        {
            //Arrange
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            IConfirmationLink _expected = new ConfirmationLink(username, Guid.NewGuid(), DateTime.Now.ToUniversalTime());
            _sqlDAO.CreateConfirmationLink(_expected);

            string linkUrl = linkUrl = $"{url}/Account/Verify?t={_expected.UniqueIdentifier}";

            //Act
            IConfirmationLink _confirmationLink = _registrationService.GetConfirmationLink(linkUrl);

            //Assert
            Assert.Equal(_expected, _confirmationLink);
        }

        [Theory]
        [InlineData("HunterS@gmail.com")]
        [InlineData("pammypoor@gmail.com")]
        [InlineData("Fez@hotmail.com")]
        public void RemoveConfirmationLink(string username)
        {
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            Guid guid = Guid.NewGuid();
            DateTime now = DateTime.Now.ToUniversalTime();
            IConfirmationLink link = new ConfirmationLink(username, guid, now);
            _sqlDAO.CreateConfirmationLink(link);

            //Act
            List<string> results = _registrationService.RemoveConfirmationLink(link);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "superSecret")]
        [InlineData("tstingtesting@hotmail.com", "bonjourApple")]
        public void GetUserFromLink(string email, string passphrase)
        {
            ISqlDAO _sqlDAO = new InMemorySqlDAO();
            IMessageBank messageBank = new MessageBank();
            ILogService _logService = new LogService(_sqlDAO, messageBank);
            IRegistrationService _registrationService = new RegistrationService(_sqlDAO, _logService);
            Guid guid = Guid.NewGuid();
            DateTime now = DateTime.Now.ToUniversalTime();
            IAccount expected = new Account(email, email, passphrase, "User", true, false);
            IConfirmationLink link = new ConfirmationLink(email, guid, now);

            _sqlDAO.CreateAccount(expected);
            _sqlDAO.CreateConfirmationLink(link);

            //Act
            IAccount result = _registrationService.GetUserFromConfirmationLink(link);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
