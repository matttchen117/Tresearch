using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;
using TrialByFire.Tresearch.UserManagement;
using Xunit;

namespace TrialByFire.Managers.Test
{
    public class AccountManagerShould
    {
        string SqlConnectionString = "JESSIES-RAZER-B;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";

        [Theory]
        [InlineData("jess@gmail.com", "test", "User")]
        public void CreateAccount(string email, string passphrase, string authorizationLevel)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isCreated = accountManager.CreateAccount(email, passphrase, authorizationLevel);

            // Assert
            Assert.True(isCreated);
        }

        [Theory]
        [InlineData("jess@gmail.com")]
        public void DeleteAccount(string username)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isEnabled = accountManager.DeleteAccount(username);

            // Assert
            Assert.Equal(isEnabled, true);
        }

        [Theory]
        [InlineData("Username", "myPassphrase")]
        public void EnableAccount(string username, string passphrase)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isEnabled = accountManager.EnableAccount(username, passphrase);

            // Assert
            Assert.Equal(true, isEnabled);
        }

        [Theory]
        [InlineData("Username")]
        public void DisableAccount(string username)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            AccountManager accountManager = new AccountManager(mssqlDAO, logService);


            // Act
            bool isDisabled = accountManager.DisableAccount(username);

            // Assert
            Assert.Equal(isDisabled, true);
        }
    }
}
