using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;
using Xunit;

namespace TrialByFire.Managers.Test
{
    public class ArchivingManagerShould
    {
        string SqlConnectionString = "Server=DESKTOP-F0O7ECC;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = ConfigurationManager.AppSettings.Get("FilePath");
        string Destination = ConfigurationManager.AppSettings.Get("Destination");


        [Fact]
        public async void ArchiveTheLogs()
        {
            // Triple A Format
            Destination = ConfigurationManager.AppSettings.Get("SqlConnectionString");

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO(SqlConnectionString);
            LogService logService = new LogService(mssqlDAO);
            ArchivingManager archivingManager = new ArchivingManager(mssqlDAO, logService);
            bool expected = true;

            // Act
            Task<bool> archiveTask = archivingManager.ArchiveLogs();
            var actual = await archiveTask;

            // Assert
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("2021-12-15 04:27:00")]
        public async void ArchiveTheLogsAtThisTime(string sArchiveTime)
        {
            // Triple A Format

            // Arrange
            SqlDAO mssqlDAO = new SqlDAO();
            LogService logService = new LogService(mssqlDAO);
            ArchivingManager archivingManager = new ArchivingManager(mssqlDAO, logService);
            bool expected = true;

            // Act
            Task<bool> archiveTask = archivingManager.ArchiveLogs(sArchiveTime);
            var actual = await archiveTask;

            // Assert
            Assert.Equal(actual, expected);
        }
    }
}