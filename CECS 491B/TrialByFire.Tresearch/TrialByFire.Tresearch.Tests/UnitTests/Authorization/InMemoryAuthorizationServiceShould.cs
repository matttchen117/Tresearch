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
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authorization
{
    public class InMemoryAuthorizationServiceShould : InMemoryTestDependencies
    {
        public InMemoryAuthorizationServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "user", "user", "success")]
        [InlineData("billy@yahoo.com", "admin", "user", "success")]
        [InlineData("joe@outlook.com", "user", "admin", "Database: You are not authorized to perform this operation.")]
        [InlineData("bob@yahoo.com", "user", "user", "Database: The account was not found or it has been disabled.")]
        [InlineData("harry@yahoo.com", "user", "user", "Database: Please confirm your account before attempting to login.")]
        public void VerifyThatTheUserIsAuthorized(string username, string authorizationLevel, string requiredAuthLevel, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, authorizationLevel);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);

            // Act
            string result = authorizationService.VerifyAuthorized(rolePrincipal, requiredAuthLevel);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
