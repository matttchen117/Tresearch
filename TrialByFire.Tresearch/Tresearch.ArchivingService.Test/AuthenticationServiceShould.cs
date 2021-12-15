using Xunit;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Services;

namespace Tresearch.Services.Tests
{
    public class AuthenticationServiceShould
    {
        string SqlConnectionString = ConfigurationManager.AppSettings.Get("SqlConnectionString");

        [Theory]
        [InlineData("pammypoor@gmail.com", "myPassword", "System Admin")]
        public void GetAccount(string username, string passphrase, string authenticationLevel)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AuthenticationService authenticationService = new AuthenticationService(mssqlDAO, logService);
            Account expected = new Account(username, username, passphrase, authenticationLevel);


            // Act
            Account actual = authenticationService.GetAccount(username, passphrase);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
