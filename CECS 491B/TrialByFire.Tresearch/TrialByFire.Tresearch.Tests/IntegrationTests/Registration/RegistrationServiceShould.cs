using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationServiceShould
    {
        public string SqlConnectionString = "Data Source=tresearchstudentserver.database.windows.net;Initial Catalog=tresearchStudentServer;User ID=tresearchadmin;Password=CECS491B!Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public ISqlDAO _sqlDAO { get; set; }

        public ILogService _logService { get; set; }
        public IMessageBank _messageBank { get; set; }

        public IRegistrationService _registrationService { get; set; }

        public RegistrationServiceShould()
        {
            _messageBank = new MessageBank();
            _sqlDAO = new SqlDAO(SqlConnectionString, _messageBank);
            _logService = new SqlLogService(_sqlDAO);
            _registrationService = new RegistrationService(_sqlDAO, _logService);
        }


        [Theory]
        [InlineData("pammypoor+serviceRegisterUser1@gmail.com", "myRegisterPassword")]
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange 
            IAccount account = new Account(email, passphrase);

            //Act

            List<string> results = _registrationService.CreatePreConfirmedAccount(account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }


        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor-203c", "myPassword", "U", true, false, "www.Tresearch.systems/")]
        public void CreateTheLink(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed, string baseUrl)
        {
            //Arrange
            IAccount _account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);

            //Act
            List<string> results = _registrationService.CreateConfirmation(email, baseUrl);
            string guidString = results.First().Substring(results.First().LastIndexOf('=')+1);


            //Assert
            Assert.Equal(36, guidString.Length);        // GUID contains 36 characters
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor-203c", "myPassword", "U", true, false)]
        public void ConfirmTheUser(string email, string username, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            IAccount _account = new Account(email, username, passphrase, authenticationLevel, status, confirmed);

            //Act
            List<string> results = _registrationService.ConfirmAccount(_account);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
    }
}
