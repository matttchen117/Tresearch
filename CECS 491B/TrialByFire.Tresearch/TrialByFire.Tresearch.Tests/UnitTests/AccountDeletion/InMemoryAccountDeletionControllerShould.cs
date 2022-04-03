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

namespace TrialByFire.Tresearch.Tests.UnitTests.AccountDeletion
{
    public class InMemoryAccountDeletionControllerShould : TestBaseClass
    {

        public InMemoryAccountDeletionControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IAccountDeletionService, AccountDeletionService>();
            TestServices.AddScoped<IAccountDeletionManager, AccountDeletionManager>();
            TestServices.AddScoped<IAccountDeletionController, AccountDeletionController>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        //might need to add more
        [InlineData("grizzly@gmail.com", "user", "200: Server: Account Deletion Successful.")]
        [InlineData("salewa@gmail.com", "admin", "200: Server: Account Deletion Successful.")]
        [InlineData("violetKeyCard@gmail.com", "admin", "500: Database: The Account was not found.")]


        public async Task DeleteTheUserAsync(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }



            IAccountDeletionController accountDeletionController = TestProvider.GetService<IAccountDeletionController>();
            string[] expecteds = expected.Split(": ");
            ObjectResult expectedResult = new ObjectResult(expecteds[2]) { StatusCode = Convert.ToInt32(expecteds[0]) };


            // Act
            IActionResult result = await accountDeletionController.DeleteAccountAsync().ConfigureAwait(false);
            var objectResult = result as ObjectResult;


            // Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);

        }
    }
}
