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

        /*
        [Theory]
        [InlineData("jessie@gmail.com", 69422, 69420, "Sauteeing ", "Preparing food on a stove", true, "jessie@gmail.com", "jessie@gmail.com", "user", "200: Server: success")]
        [InlineData("larry@gmail.com", 100000, 100001, "Title 1", "Summary 1", false, "larry@gmail.com", "larry@gmail.com", "guest", "403: Database: You are not authorized to perform this operation.")]
        public async Task CreateTheNode(string username, long nodeID, long parentID, string nodeTitle, string summary, bool visibility,
            string accountOwner, string currentIdentity, string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ICreateNodeManager createNodeManager = TestProvider.GetService<ICreateNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(nodeID, parentID, nodeTitle, summary, visibility, false, accountOwner);
            Account account = new Account(username, "jessie123", "user");

            //Act
            string result = await createNodeManager.CreateNodeAsync(account, node, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
        */
    }
}
