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
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            14, 12, "jelazo@live.com", "user", "200: Server: Delete Node Success")]
        public async Task DeleteTheNode(string userhash, long nodeID, long parentID, string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userhash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IDeleteNodeController deleteNodeController = TestProvider.GetService<IDeleteNodeController>();
            Node node = new Node(userhash, nodeID, parentID, "", "", DateTime.UtcNow, true, false);

            // Act
            ActionResult<string> response = await deleteNodeController.DeleteNodeAsync(node).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, response.Value);
        }
    }
}
