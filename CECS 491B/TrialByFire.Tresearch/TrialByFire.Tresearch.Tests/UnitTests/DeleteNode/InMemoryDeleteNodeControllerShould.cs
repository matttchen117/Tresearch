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

namespace TrialByFire.Tresearch.Tests.UnitTests.DeleteNode
{
    public class InMemoryDeleteNodeControllerShould : TestBaseClass
    {
        public InMemoryDeleteNodeControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IDeleteNodeService, DeleteNodeService>();
            TestServices.AddScoped<IDeleteNodeManager, DeleteNodeManager>();
            TestServices.AddScoped<IDeleteNodeController, DeleteNodeController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("jessie@gmail.com", 69420, 69419, "jessie@gmail.com", "user", "200: Server: Delete Node Success")]
        [InlineData("viet@gmail.com", 69420, 69419, "jessie@gmail.com", "user", "403: Database: You are not authorized to perform this operation.")]
        [InlineData("jessie@gmail.com", 80085, 80084, "jessie@gmail.com", "user", "504: Database: The node was not found.")]
        public async Task DeleteTheNode(string username, long nodeID, long parentID, string currentIdentity, string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            IDeleteNodeController deleteNodeController = TestProvider.GetService<IDeleteNodeController>();
            string[] expects = expected.Split(": ");
            ObjectResult expectedResult = new ObjectResult(expects[2])
            {
                StatusCode = Convert.ToInt32(expects[0])
            };
            IAccount account = new UserAccount(username, "jessie123", "user");

            //Act
            IActionResult result = await deleteNodeController.DeleteNodeAsync(account, nodeID, parentID).ConfigureAwait(false);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
