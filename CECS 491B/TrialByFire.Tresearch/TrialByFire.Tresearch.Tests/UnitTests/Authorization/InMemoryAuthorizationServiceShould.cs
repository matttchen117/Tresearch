using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthorizationTests
{
    public class InMemoryAuthorizationServiceShould : InMemoryTestDependencies
    {
        public InMemoryAuthorizationServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "user", "user", "success")]
        [InlineData("billy@yahoo.com", "admin", "user", "success")]
        [InlineData("joe@outlook.com", "user", "admin", "Data: You are not authorized to perform this operation.")]
        [InlineData("bob@yahoo.com", "user", "user", "Database: No active session found. Please login and try again.")]
        [InlineData("harry@yahoo.com", "user", "user", "Database: No active session found. Please login " +
            "and try again.")]
        public void VerifyThatTheUserIsAuthorized(string username, string role, string requiredRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, role);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);

            // Act
            string result = authorizationService.Authorize(rolePrincipal, requiredRole);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
