using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.CreateNode
{
    public class CreateNodeControllerShould : TestBaseClass
    {
        
        public CreateNodeControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestServices.AddScoped<ICreateNodeManager, CreateNodeManager>();
            TestServices.AddScoped<ICreateNodeController, CreateNodeController>();
            TestProvider = TestServices.BuildServiceProvider();
        }
        
        [Theory]
        [InlineData("75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6",
            1702, "Over-head Press", "OHP for ORM", "jelazo@live.com", "user", "200: Server: Create Node Success")]
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
