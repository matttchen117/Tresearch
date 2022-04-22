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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.DeleteNode
{
    public class DeleteNodeManagerShould : TestBaseClass
    {
        public DeleteNodeManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<IDeleteNodeService, DeleteNodeService>();
            TestServices.AddScoped<IDeleteNodeManager, DeleteNodeManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("jessie@gmail.com", 69420, 69419, "jessie@gmail.com", "user", "200: Server: Delete Node Success")]
        [InlineData("viet@gmail.com", 69420, 69419, "jessie@gmail.com", "user", "403: Database: You are not authorized to perform this operation.")]
        [InlineData("jessie@gmail.com", 80085, 80084, "jessie@gmail.com", "user", "504: Database: The node was not found.")]
        public async Task DeleteTheNode(string username, long nodeID, long parentID, string currentIdentity,
            string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IDeleteNodeManager deleteNodeManager = TestProvider.GetService<IDeleteNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Account account = new Account(username, "jessie123", "user");

            //Act
            string result = await deleteNodeManager.DeleteNodeAsync(account, nodeID, parentID, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}