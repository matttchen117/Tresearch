using System;
using System.Configuration;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;
using Xunit;

namespace Tresearch.Services.Test
{
    
    public class ArchivingServiceShould
    {
        string SqlConnectionString = "Server=DESKTOP-F0O7ECC;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
        string FilePath = ConfigurationManager.AppSettings.Get("FilePath");
        string Destination = ConfigurationManager.AppSettings.Get("Destination");

        [Fact]
        public void ArchiveTheLogs()
        {
            // Triple A Format

            // Arrange
            MSSQLDAO mssqlDAO = new MSSQLDAO();
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
