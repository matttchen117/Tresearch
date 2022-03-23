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

namespace TrialByFire.Tresearch.Tests.UnitTests.Authorization
{
    public class InMemoryAuthorizationServiceShould : TestBaseClass
    {
        public InMemoryAuthorizationServiceShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("aarry@gmail.com", "user", "user", "200: Server: success")]
        [InlineData("barry@gmail.com", "admin", "user", "200: Server: success")]
        [InlineData("carry@gmail.com", "user", "admin", "403: Database: You are not authorized to perform this operation.")]
        [InlineData("darry@gmail.com", "user", "user", "404: Database: The account was not found or it has been disabled.")]
        [InlineData("earry@gmail.com", "user", "user", "401: Database: Please confirm your account before attempting to login.")]
        public async Task VerifyThatTheUserIsAuthorized(string username, string authorizationLevel, string requiredAuthLevel, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, authorizationLevel);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IAuthorizationService authorizationService = TestApp.Services.GetService<IAuthorizationService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await authorizationService.VerifyAuthorizedAsync(requiredAuthLevel, 
                cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
