using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Logout
{
    public class InMemoryLogoutControllerShould : TestBaseClass 
    {
        public InMemoryLogoutControllerShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestBuilder.Services.AddScoped<ILogoutService, LogoutService>();
            TestBuilder.Services.AddScoped<ILogoutManager, LogoutManager>();
            TestBuilder.Services.AddScoped<ILogoutController, LogoutController>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("guest", "guest", "401: Server: No active session found. Please login and try again.")]
        [InlineData("aarry@gmail.com", "user", "200: Server: success")]
        public async Task LogTheUserOut(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if(!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            ILogoutController logoutController = TestApp.Services.GetService<ILogoutController>();
            string[] expecteds = expected.Split(": ");
            ObjectResult expectedResult = new ObjectResult(expecteds[2])
            { StatusCode = Convert.ToInt32(expecteds[0]) };

            // Act
            IActionResult result = await logoutController.Logout().ConfigureAwait(false);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
