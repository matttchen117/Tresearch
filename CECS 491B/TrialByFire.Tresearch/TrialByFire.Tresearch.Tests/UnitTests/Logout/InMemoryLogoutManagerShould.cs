using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Logout
{
    public class InMemoryLogoutManagerShould : InMemoryTestDependencies
    {
        public InMemoryLogoutManagerShould() : base()
        {
        }

        [Theory]
        [InlineData("guest", "guest", "Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user", "success")]
        public void LogTheUserOut(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            ILogoutService logoutService = new LogoutService(sqlDAO, logService, messageBank, rolePrincipal);
            ILogoutManager logoutManager = new LogoutManager(sqlDAO, logService, messageBank, 
                rolePrincipal, logoutService);

            // Act
            string result = logoutManager.Logout();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
