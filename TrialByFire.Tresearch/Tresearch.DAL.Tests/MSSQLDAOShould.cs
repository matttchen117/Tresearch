using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using Xunit;

namespace TrialByFire.DAL.Tests
{
    public class MSSQLDAOShould
    {

        string SqlConnectionString = "Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = @"C:\Work\Logs";
        string Destination = @"C:\Work";

        [Theory]
        [InlineData("bob@gmail.com", "abcdef123456", "bob@gmail.com", "abcdef123456", "User", false)]
        [InlineData("larry@gmail.com", "abcdef123456", "larry@gmail.com", "abcdef123456", "User", true)]
        public void GetTheAccount(string email, string passphrase, string expectedEmail, 
            string expectedPassphrase, string expectedAuthorizationLevel, bool expectedResult)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            var expected = new Account(expectedEmail, expectedPassphrase, expectedAuthorizationLevel);

            // Act
            var actual = mssqlDAO.GetAccount(email, passphrase);

            // Assert
            Assert.Equal(expected.Equals(actual), expectedResult);
        }

        [Fact]
        public void CreateTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
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
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);


            // Act
            var actual = mssqlDAO.UpdateAccount("bob1@gmail.com", "123456abcdef", "larry@gmail.com", "System Admin");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DeleteTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);


            // Act
            var actual = mssqlDAO.DeleteAccount("bob@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DisableTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);


            // Act
            var actual = mssqlDAO.DisableAccount("larry@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void EnableTheAccount()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);


            // Act
            var actual = mssqlDAO.EnableAccount("larry@gmail.com", "larry@gmail.com");

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
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
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
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString, FilePath, Destination);

            // Act
            var actual = mssqlDAO.Archive();

            // Assert
            Assert.True(actual);
        }
    }
}