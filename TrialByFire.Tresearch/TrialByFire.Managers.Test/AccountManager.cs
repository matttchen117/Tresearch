using Xunit;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;
using System.Configuration;

namespace Tresearch.Managers.Tests
{
    public class AccountManagerShould
    {
        string SqlConnectionString = ConfigurationManager.AppSettings.Get("SqlConnectionString");

        [Theory]
        [InlineData("gregory@gmail.com", "scrumGitSome", "User")]
        public void CreateAccount(string username, string passphrase, string authorizationLevel)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isAdded = accountManager.CreateAccount(username, passphrase, authorizationLevel);

            // Assert
            Assert.True(isAdded);
        }

        [Theory]
        [InlineData("cameron@gmail.com")]
        public void DeleteAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);
            


            // Act
            bool isDeleted = accountManager.DeleteAccount(username);

            // Assert
            Assert.True(isDeleted);
        }

        [Theory]
        [InlineData("austin@gmail.com", "austin@gmail.com")]
        public void EnableAccount(string username, string email)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isEnabled = accountManager.EnableAccount(username, email);

            // Assert
            Assert.True(isEnabled);
        }

        [Theory]
        [InlineData("ally@gmail.com")]
        public void DisableAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isDisabled = accountManager.DisableAccount(username);

            // Assert
            Assert.True(isDisabled);
        }
    }
}
