using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using Xunit;

namespace Tresearch.Logging.Tests
{
    public class LogServiceShould
    {
        string SqlConnectionString = "Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";

        [Theory]
        [InlineData("Jan 1, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Jan 1, 2022", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Dec 1, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Nov 8, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        [InlineData("Nov 7, 2021", "Info", "larry@gmail.com", "DataStore", "This is a test.")]
        public void CreateTheLog(string timeString, string level, string username, string category, string description)
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            DateTime timeStamp = DateTime.Parse(timeString).ToUniversalTime();
            bool expected = true;

            // Act
            var actual = logService.CreateLog(timeStamp, level, username, category, description);

            // Assert
            Assert.Equal(actual, expected);
        }
    }
}