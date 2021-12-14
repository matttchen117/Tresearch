using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;
using Xunit;

namespace Tresearch.Services.Test
{
    
    public class ArchivingServiceShould
    {
        string SqlConnectionString = "Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = @"C:\Work\Logs";
        string Destination = @"C:\Work";

        [Fact]
        public void ArchiveTheLogs()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO(SqlConnectionString, FilePath, Destination);
            LogService logService = new LogService(mssqlDAO);
            ArchivingService archivingService = new ArchivingService(mssqlDAO, logService);
            bool expected = true;

            // Act
            var actual = archivingService.Archive();

            // Assert
            Assert.Equal(actual, expected);
        }

    }
}
