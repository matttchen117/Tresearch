using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Logout
{
    public class InMemoryLogoutServiceShould : InMemoryTestDependencies
    {
        public InMemoryLogoutServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("aarry@gmail.com", "user", "200: Server: success")]
        public void LogTheUserOut(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            ILogoutService logoutService = new LogoutService(sqlDAO, logService, messageBank, rolePrincipal);
            

            // Act
            string result = logoutService.Logout();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
