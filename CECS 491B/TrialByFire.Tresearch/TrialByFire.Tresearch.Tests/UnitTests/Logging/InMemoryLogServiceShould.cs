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

namespace TrialByFire.Tresearch.Tests.UnitTests.Logging
{
    public class InMemoryLogServiceShould : TestBaseClass
    {
        public InMemoryLogServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ILogService, LogService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.")]
        public async Task CreateTheLogAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash, 
            string category, string description)
        {
            // Arrange
            DateTime timestamp = new DateTime(year, month, day, hour, minute, second).ToUniversalTime();
            ILogService logService = TestProvider.GetService<ILogService>();
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

            // Act
            ILog log = await logService.CreateLogAsync(timestamp, level, category, description)
                .ConfigureAwait(false);

            // Assert
            Assert.NotNull(log);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user", "Server",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.")]
        public async Task CreateTheLogAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash, 
            string category, string description)
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
            ILogService logService = TestProvider.GetService<ILogService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            ILog log = await logService.CreateLogAsync(timestamp, level, category, description, 
                cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.NotNull(log);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "AnalyticLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "AnalyticLogs", "200: Server: Log success.")]
        public async Task StoreTheLogAsync(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole,  string userHash, 
            string category, string description, string destination, string expected)
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
            ILogService logService = TestProvider.GetService<ILogService>();
            ILog log = await logService.CreateLogAsync(timestamp, level, category, description).ConfigureAwait(false);

            // Act
            string result = await logService.StoreLogAsync(log, destination).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "ArchiveLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "user",
            "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
            "Server", "Log success.", "AnalyticLogs", "200: Server: Log success.")]
        [InlineData(2022, 4, 2, 5, 0, 0, "Info", "drakat7@gmail.com", "admin",
            "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32",
            "Server", "Log success.", "AnalyticLogs", "200: Server: Log success.")]
        public async Task StoreTheLogAsyncWithin5Seconds(int year, int month, int day, int hour, int minute,
            int second, string level, string currentIdentity, string currentRole, string userHash, 
            string category, string description, string destination, string expected)
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
            ILogService logService = TestProvider.GetService<ILogService>();
            ILog log = await logService.CreateLogAsync(timestamp, level, category, description).ConfigureAwait(false);
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
