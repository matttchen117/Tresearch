using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Logging
{
    public class LogServiceShould : TestBaseClass
    {
        public LogServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ILogService, LogService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task CreateTheLogAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category, string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();


            // Act
            ILog log = await logService.CreateLogAsync(timestamp, level, username, authorizationLevel, 
                category, description).ConfigureAwait(false);

            // Assert
            Assert.NotNull(log);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task CreateTheLogAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category, string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            ILog log = await logService.CreateLogAsync(timestamp, level, username, authorizationLevel,
                category, description, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.NotNull(log);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.",
            "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.",
            "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.",
            "AnalyticLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.",
            "AnalyticLogs", "200: Server: Log success.")]
        public async Task StoreTheLogAsync(int year, int month, int day, int hour, int minute, 
            int second, string level, string username, string authorizationLevel, string category, 
            string description, string destination, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();
            ILog log = await logService.CreateLogAsync(timestamp, level, username, authorizationLevel,
                category, description).ConfigureAwait(false);

            // Act
            string result = await logService.StoreLogAsync(log, destination).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.",
            "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.",
            "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.",
            "AnalyticLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.",
            "AnalyticLogs", "200: Server: Log success.")]
        public async Task StoreTheLogAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category,
            string description, string destination, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();
            ILog log = await logService.CreateLogAsync(timestamp, level, username, authorizationLevel,
                category, description).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await logService.StoreLogAsync(log, destination, 
                cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
