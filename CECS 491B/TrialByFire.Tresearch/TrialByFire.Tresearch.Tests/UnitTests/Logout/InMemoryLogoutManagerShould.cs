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

namespace TrialByFire.Tresearch.Tests.UnitTests.Logout
{
    public class InMemoryLogoutManagerShould : TestBaseClass
    {
        public InMemoryLogoutManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ILogoutManager, LogoutManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("guest", "guest", "", "401: Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user",
                    "AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D",
                    "200: Server: Logout success.")]
        public async Task LogTheUserOutAsync(string currentIdentity, string currentRole, 
            string userHash, string expected)
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
            ILogoutManager logoutManager = TestProvider.GetService<ILogoutManager>();

            // Act
            string result = await logoutManager.LogoutAsync().ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("guest", "guest", "", "401: Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user",
                    "AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 
                    "200: Server: Logout success.")]
        public async Task LogTheUserOutAsyncWithin5Seconds(string currentIdentity, string currentRole, 
            string userHash, string expected)
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
