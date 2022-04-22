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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.TreeManagement
{
    public class TreeManagementControllerShould : TestBaseClass
    {
        public TreeManagementControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ITreeManagementService, TreeManagementService>();
            TestServices.AddScoped<ITreeManagementManager, TreeManagementManager>();
            TestServices.AddScoped<ITreeManagementController, TreeManagementController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        /*[InlineData("27a285fe87f1d0afb44f2310824f49bbf1aaea02b856d314412119142ecfbb46ece7dcadc6c516c4d3918532df9375bd9f377e395143f0a29aed88654bff1c95", "jessie@gmail.com", "user", "200: Server: Get Nodes Success")]
        [InlineData("AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", "jessie@gmail.com", "user", "200: Server: Get Nodes Success")]*/
        [InlineData("AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", "jelazo@live.com", "user","200: Server: Success")]
        public async Task GetTheNodes(string userhash, string username, string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITreeManagementController treeManagementController = TestProvider.GetRequiredService<ITreeManagementController>();
            string[] expects = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(expects[2])
            { StatusCode = Convert.ToInt32(expects[0])};

            //Act
            IActionResult results = await treeManagementController.GetNodesAsync(userhash);
            var result = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            //Assert.Equal(expectedResult.Value, expected.Value);
        }
    }
}
