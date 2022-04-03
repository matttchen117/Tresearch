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
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "Server", "Log success.", "200: Server: Log success.")]
        public async Task StoreTheLogAsync(int year, int month, int day, int hour, int minute, 
            int second, string level, string username, string category, string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();


            // Act
            string result = await logService.StoreLogAsync(timestamp, level, username, category, 
                description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "Server", "Log success.", "200: Server: Log success.")]
        public async Task StoreTheLogAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string username, string category, string description, string expected)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));

            // Act
            string result = await logService.StoreLogAsync(timestamp, level, username, category,
                description, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
