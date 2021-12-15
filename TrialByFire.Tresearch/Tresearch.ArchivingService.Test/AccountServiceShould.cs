using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;
using Xunit;

namespace TrialByFire.Services.Test
{
    public class AccountServiceShould
    {
        string SqlConnectionString = ConfigurationManager.AppSettings.Get("SqlConnectionString");

        [Theory]
        [InlineData("jessie@gmail.com", "test", "User")]
        public void CreateAccount(string email, string passphrase, string authorizationLevel)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountService accountService = new AccountService(mssqlDAO, logService);


            // Act
            bool isCreated = accountService.CreateAccount(email, passphrase, authorizationLevel);

            // Assert
            Assert.True(isCreated);
        }

        

        [Theory]
        [InlineData("williams@gmail.com", "test2", "jessie2@gmail.com", "")]
        public void UpdateAccount(string username, string newPassphrase, string newEmail, string newAuthorizationLevel)
        {
            //Triple A Format

            //Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountService accountService = new AccountService(mssqlDAO, logService);
            
            //Act
            bool isUpdated = accountService.UpdateAccount(username, newPassphrase, newEmail, newAuthorizationLevel);

            //Assert
            Assert.True(isUpdated);
        }

        [Theory]
        [InlineData("natalie@gmail.com", "processor")]
        public void EnableAccount(string username, string passphrase)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountService accountService = new AccountService(mssqlDAO, logService);


            // Act
            bool isEnabled = accountService.EnableAccount(username, passphrase);

            // Assert
            Assert.True(isEnabled);
        }

        [Theory]
        [InlineData("kim@gmail.com")]
        public void DisableAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountService accountService = new AccountService(mssqlDAO, logService);


            // Act
            bool isDisabled = accountService.DisableAccount(username);

            // Assert
            Assert.True(isDisabled);
        }

        [Theory]
        [InlineData("novak@gmail.com")]
        public void DeleteAccount(string username)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            LogService logService = new LogService(mssqlDAO);
            AccountService accountService = new AccountService(mssqlDAO, logService);


            // Act
            bool isDeleted = accountService.DeleteAccount(username);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
