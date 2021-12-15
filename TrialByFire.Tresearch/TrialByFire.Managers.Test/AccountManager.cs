using Xunit;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;

namespace Tresearch.Managers.Tests
{
    public class AccountManagerShould
    {
        string SqlConnectionString = "Server=DESKTOP-F0O7ECC;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";

        [Theory]
        [InlineData("pammy@gmail.com", "Password", "User")]
        public void CreateAccount(string username, string passphrase, string authorizationLevel)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isAdded = accountManager.CreateAccount(username, passphrase, authorizationLevel);

            // Assert
            Assert.True(isAdded);
        }

        [Theory]
        [InlineData("larry@gmail.com")]
        public void DeleteAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);
            


            // Act
            bool isDeleted = accountManager.DeleteAccount(username);

            // Assert
            Assert.True(isDeleted);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "pammypoor@gmail.com")]
        public void EnableAccount(string username, string email)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isEnabled = accountManager.EnableAccount(username, email);

            // Assert
            Assert.True(isEnabled);
        }

        [Theory]
        [InlineData("bob@gmail.com")]
        public void DisableAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isDisabled = accountManager.DisableAccount(username);

            // Assert
            Assert.True(isDisabled);
        }
    }
}
