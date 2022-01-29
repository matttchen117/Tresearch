using Xunit;
using System.Configuration;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;

namespace Tresearch.Services.Tests
{
    public class AuthorizationServiceShould
    {
        string SqlConnectionString = "Server=DESKTOP-F0O7ECC;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";

        [Theory]
        [InlineData("bob@gmail.com", "bob1@gmail.com" ,"abcdef123456", "User")]
        public void GetAccountAuthLevel(string email, string username, string passphrase, string authenticationLevel)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AuthorizationService authorizationService = new AuthorizationService(mssqlDAO, logService);
            Account a = new Account(email, username, passphrase, authenticationLevel);

            // Act
           bool isAuthorized = authorizationService.GetAccountAuthLevel(a, authenticationLevel);

            // Assert
            Assert.True(isAuthorized);
        }
    }
}
