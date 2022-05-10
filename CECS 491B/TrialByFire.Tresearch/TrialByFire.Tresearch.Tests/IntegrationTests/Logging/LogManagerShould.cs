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
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToAnalyticTableAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash,
            string category, string description, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);

            // Act
            string result = await logManager.StoreAnalyticLogAsync(timestamp, enumLevel, enumCategory,
                description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToAnalyticTableAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash,
            string category, string description, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await logManager.StoreAnalyticLogAsync(timestamp, enumLevel, enumCategory,
                description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToArchiveTableAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash,
            string category, string description, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);


            // Act
            string result = await logManager.StoreArchiveLogAsync(timestamp, enumLevel, enumCategory,
                description).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "200: Server: Log success.")]
        public async Task LogToArchiveTableAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash,
            string category, string description, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogManager logManager = TestProvider.GetService<ILogManager>();
            Enum.TryParse(level, out ILogManager.Levels enumLevel);
            Enum.TryParse(category, out ILogManager.Categories enumCategory);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await logManager.StoreArchiveLogAsync(timestamp, enumLevel, enumCategory,
                description)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
