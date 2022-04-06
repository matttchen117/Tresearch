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
        [InlineData("jessie@gmail.com", 69420, 69419, "Cooking", "Concepts of Preparing Food", true, "jessie@gmail.com", "jessie@gmail.com", "guest", "200: Server: success")]
        [InlineData("larry@gmail.com", 100000, 100001, "Title 1", "Summary 1", false, "larry@gmail.com", "larry@gmail.com", "guest", "409: Database: Node Already Exists")]
        public async Task CreateTheNode(string username, long nodeID, long parentID, string nodeTitle, string summary, bool visibility,
            string accountOwner, string currentIdentity, string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            ICreateNodeManager createNodeManager = TestProvider.GetService<ICreateNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(nodeID, parentID, nodeTitle, summary, visibility, accountOwner);

            //Act
            string result = await createNodeManager.CreateNodeAsync(username, node, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
