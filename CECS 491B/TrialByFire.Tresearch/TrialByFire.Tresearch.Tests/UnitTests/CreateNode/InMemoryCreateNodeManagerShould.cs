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
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.CreateNode
{
    public class InMemoryCreateNodeManagerShould : TestBaseClass
    {
        public InMemoryCreateNodeManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestServices.AddScoped<ICreateNodeManager, CreateNodeManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            16, "Bench", "Bench for ORM", true, false, "jelazo@live.com", "user", "200: Server: Create Node Success")]
        public async Task CreateTheNode(string userhash, long parentID, string nodeTitle, string summary, bool visibility, bool deleted,
            string accountOwner, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, accountOwner, currentRole, userhash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            ICreateNodeManager createNodeManager = TestProvider.GetService<ICreateNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(userhash, 0, parentID, nodeTitle, summary, DateTime.UtcNow, visibility, deleted);

            // Act
            IResponse<string> result = await createNodeManager.CreateNodeAsync(userhash, node, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result.Data);
        }

    }
}
