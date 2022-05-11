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

namespace TrialByFire.Tresearch.Tests.UnitTests.CreateNode
{
    public class InMemoryCreateNodeControllerShould : TestBaseClass
    {
        public InMemoryCreateNodeControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestServices.AddScoped<ICreateNodeManager, CreateNodeManager>();
            TestServices.AddScoped<ICreateNodeController, CreateNodeController>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            14, "OHP", "OHP for ORM", "jelazo@live.com", "user", "200: Server: Create Node Success")]
        public async Task CreateTheNodeAsync(string userhash, long parentID, string nodeTitle, string summary, string username, string role, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, role, userhash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            ICreateNodeController _createNodeController = TestProvider.GetService<ICreateNodeController>();

            // Act
            ActionResult<string> response = await _createNodeController.CreateNodeAsync(userhash, parentID, nodeTitle, summary);

            // Assert
            Assert.Equal(expected, response.Value);
        }
        
    }
}

