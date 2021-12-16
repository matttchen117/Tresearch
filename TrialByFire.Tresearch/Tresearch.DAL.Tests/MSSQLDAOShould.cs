using System;
using System.Configuration;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using Xunit;

namespace TrialByFire.DAL.Tests
{
    public class MSSQLDAOShould
    {

        string SqlConnectionString = ConfigurationManager.AppSettings.Get("SqlConnectionString");
        string FilePath = ConfigurationManager.AppSettings.Get("FilePath");
        string Destination = ConfigurationManager.AppSettings.Get("Destination");

        [Theory]
        [InlineData("federer3@gmail.com", "swissCheese20", "federer@gmail.com", "swissCheese20", "User", false)]
        [InlineData("gibbs@gmail.com", "likeClockwork", "gibbs@gmail.com", "likeClockwork", "User", true)]
        public void GetTheAccount(string email, string passphrase, string expectedEmail, 
            string expectedPassphrase, string expectedAuthorizationLevel, bool expectedResult)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            var expected = new Account(expectedEmail, expectedPassphrase, expectedAuthorizationLevel);

            // Act
            var actual = mssqlDAO.GetAccount(email, passphrase);

            // Assert
            Assert.Equal(expectedResult, expected.Equals(actual));
        }

        [Fact]
        public void CreateTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            Account account = new Account("bob@gmail.com", "abcdef123456", "User");

            // Act
            var actual = mssqlDAO.CreateAccount(account);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void UpdateTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();


            // Act
            var actual = mssqlDAO.UpdateAccount("greg@gmail.com", "hexagons333", "agatha@gmail.com", "System Admin");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DeleteTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();


            // Act
            var actual = mssqlDAO.DeleteAccount("c00lCoder@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DisableTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();


            // Act
            var actual = mssqlDAO.DisableAccount("trenton@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void EnableTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();


            // Act
            var actual = mssqlDAO.EnableAccount("riley@gmail.com", "riley@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData("Jan 1, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Jan 1, 2022", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Dec 1, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Nov 8, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Nov 7, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        public void StoreTheLog(string timeString, string level, string username, string category, string description)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
            DateTime timeStamp = DateTime.Parse(timeString).ToUniversalTime();
            Log log = new Log(timeStamp, level, username, category, description);
            bool expected = true;

            // Act
            bool actual = mssqlDAO.StoreLog(log);

            // Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ArchiveTheLogs()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();

            // Act
            var actual = mssqlDAO.Archive();

            // Assert
            Assert.True(actual);
        }
    }
}