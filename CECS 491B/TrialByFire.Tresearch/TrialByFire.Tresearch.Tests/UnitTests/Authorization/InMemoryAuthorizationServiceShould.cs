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
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("aarry@gmail.com", "user", "user", "", true)]
        [InlineData("barry@gmail.com", "admin", "user", "", true)]
        [InlineData("carry@gmail.com", "user", "admin", "", false)]
        [InlineData("aarry@gmail.com", "user", "", "aarry@gmail.com", true)]
        [InlineData("barry@gmail.com", "admin", "", "aarry@gmail.com", false)]
        public async Task VerifyThatTheUserIsAuthorized(string username, string authorizationLevel, 
            string requiredAuthLevel, string identity, bool expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, authorizationLevel);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IAuthorizationService authorizationService = TestProvider.GetService<IAuthorizationService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            bool result = await authorizationService.VerifyAuthorizedAsync(requiredAuthLevel, 
                identity,cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
