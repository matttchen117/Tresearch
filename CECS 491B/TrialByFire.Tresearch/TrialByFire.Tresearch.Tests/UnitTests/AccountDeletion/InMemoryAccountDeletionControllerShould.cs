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
        [InlineData("grizzly@gmail.com", "user",
                    "87EC69F0AB41C3DCB31E01DCF9942D756501B421887524A1E691DFF69A698CF1D46C26B68F73DDDB29A7D2729EDDF43580BAB9A5002D2289C0C7BF4D5DB7C7AE",
                    "success")]
        [InlineData("salewa@gmail.com", "admin",
            "C53F20310CB3A9B309086CF3010D94444FC45D159CAD810F9B5C6AED9A698A0E277A3246761BA2493590B7FDDDCD7009439796F058ADBA014D0AD43086FA9B3F",
            "success")]
        [InlineData("violetKeyCard@gmail.com", "admin",
                    "D04E432CDE8F3264FA023D34AB140936D67443AE71A448E49DA1F0FA4430B20436DE69F875F74492E41221DE7510E857055EAF2D770CE87E73B3AA8B61097644",
                    "Database: The account was not found.")]

        public void DeleteTheUser(string currentIdentity, string currentRole, string userHash, 
            string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole, userHash);
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
