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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Logout
{
    public class LogoutManagerShould : TestBaseClass
    {
        public LogoutManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ILogoutService, LogoutService>();
            TestServices.AddScoped<ILogoutManager, LogoutManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("guest", "guest", "401: Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user", "200: Server: Logout success.")]
        public async Task LogTheUserOutAsync(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            ILogoutManager logoutManager = TestProvider.GetService<ILogoutManager>();

            // Act
            string result = await logoutManager.LogoutAsync().ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("guest", "guest", "401: Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user", "200: Server: Logout success.")]
        public async Task LogTheUserOutAsyncWithin5Seconds(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if(!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            ILogoutManager logoutManager = TestProvider.GetService<ILogoutManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));

            // Act
            string result = await logoutManager.LogoutAsync(cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
