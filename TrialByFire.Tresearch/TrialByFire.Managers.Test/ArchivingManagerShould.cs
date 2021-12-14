using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Managers;
using Xunit;

namespace TrialByFire.Managers.Test
{
    public class ArchivingManagerShould
    {
        string SqlConnectionString = "Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = @"C:\Work\Logs";
        string Destination = @"C:\Work";

        public void LaunchTheArchivingService()
        {

        }

        [Fact]
        public void ArchiveTheLogs()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString, FilePath, Destination);
            LogService logService = new LogService(mssqlDAO);
            ArchivingManager archivingManager = new ArchivingManager(mssqlDAO, logService);
            bool expected = true;

            // Act
            var actual = archivingManager.ArchiveLogs();

            // Assert
            Assert.Equal(actual, expected);
        }
    }
}