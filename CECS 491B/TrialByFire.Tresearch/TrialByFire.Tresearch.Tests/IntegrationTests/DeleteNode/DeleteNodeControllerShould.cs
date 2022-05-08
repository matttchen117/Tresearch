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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.DeleteNode
{
    public class DeleteNodeControllerShould : TestBaseClass
    {
        public DeleteNodeControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<IDeleteNodeService, DeleteNodeService>();
            TestServices.AddScoped<IDeleteNodeManager, DeleteNodeManager>();
            TestServices.AddScoped<IDeleteNodeController, DeleteNodeController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            34, 14, "jelazo@live.com", "user", "200: Server: Delete Node Success")]

        public async Task DeleteTheNode(string userhash, long nodeID, long parentID, string currentIdentity, string currentRole, string expected)
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
            string userName = Thread.CurrentPrincipal.Identity.Name;

            string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
            IAccount account = new UserAccount(userName, userAuthLevel);
            Node node = new Node(userhash, nodeID, parentID, "", "", DateTime.UtcNow, true, false);

            //Act
            ActionResult<string> result = await deleteNodeController.DeleteNodeAsync(node).ConfigureAwait(false);
            //var objectResult = result as ObjectResult;

            //Assert
            //Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            //Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
