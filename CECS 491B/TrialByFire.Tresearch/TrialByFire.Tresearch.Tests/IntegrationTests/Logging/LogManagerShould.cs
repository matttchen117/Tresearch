using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Logging
{
    public class LogManagerShould : TestBaseClass
    {
        public LogManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ILogService, LogService>();
            TestServices.AddScoped<ILogManager, LogManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToAnalyticTableAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category, 
            string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);

            // Act
            string result = await logManager.StoreAnalyticLogAsync(timestamp, enumLevel, username, 
                authorizationLevel, enumCategory, description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToAnalyticTableAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category,
            string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await logManager.StoreAnalyticLogAsync(timestamp, enumLevel, username, 
                authorizationLevel, enumCategory, description, cancellationTokenSource.Token)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToArchiveTableAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category,
            string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);

            // Act
            string result = await logManager.StoreArchiveLogAsync(timestamp, enumLevel, username,
                authorizationLevel, enumCategory, description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin", "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToArchiveTableAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string authorizationLevel, string category,
            string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await logManager.StoreArchiveLogAsync(timestamp, enumLevel, username,
                authorizationLevel, enumCategory, description, cancellationTokenSource.Token)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
