using System;
using System.Collections.Generic;
using System.Configuration;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using Xunit;

namespace TrialByFire.DAL.Tests
{
    public class MSSQLDAOShould
    {

        string SqlConnectionString = "Server=DESKTOP-F0O7ECC;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = "D:\\Work\\Logs";
        string Destination = "D:\\Work";

        [Theory]
        /*[InlineData("federer3@gmail.com", "swissCheese20", "federer@gmail.com", "swissCheese20", "User", false)]
        [InlineData("gibbs@gmail.com", "likeClockwork", "gibbs@gmail.com", "likeClockwork", "User", true)]*/
        public void GetTheAccount(Account account)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            var expected = new Account(expectedEmail, expectedPassphrase, expectedAuthorizationLevel);

            // Act
            var actual = mssqlDAO.GetAccount(email, passphrase);

            // Assert
            Assert.Equal(expectedResult, expected.Equals(actual));
        }

        [Theory]
        /*[InlineData("random@gmail.com", "myPassword", "User")]
        [InlineData("myRandomUsername@gmail.com", "ooooopppppp", "System Admin")]*/
        public void CreateTheAccount(List<Account> accounts)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            Account account = new Account(username, passphrase, authenticationLevel);

            // Act
            var actual = mssqlDAO.CreateAccount(account);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        /*[InlineData("greg@gmail.com", "random@gmail.com", "myPassword", "User")]
        [InlineData("greg@gmail.com", "myRandomUsername@gmail.com", "ooooopppppp", "System Admin")]*/
        public void UpdateTheAccount(List<Account> accounts)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);


            // Act
            var actual = mssqlDAO.UpdateAccount(username, newEmail, newPassphrase, newLevel);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DeleteTheAccount()
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);


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
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);


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
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);


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
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
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
            SqlDAO mssqlDAO = new SqlDAO();

            // Act
            var actual = mssqlDAO.Archive();

            // Assert
            Assert.True(actual);
        }
    }
}