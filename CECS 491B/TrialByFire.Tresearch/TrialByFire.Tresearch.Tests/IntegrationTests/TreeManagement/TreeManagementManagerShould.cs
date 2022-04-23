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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.TreeManagement
{
    public class TreeManagementManagerShould : TestBaseClass
    {
        public TreeManagementManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ITreeManagementService, TreeManagementService>();
            TestServices.AddScoped<ITreeManagementManager, TreeManagementManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("27a285fe87f1d0afb44f2310824f49bbf1aaea02b856d314412119142ecfbb46ece7dcadc6c516c4d3918532df9375bd9f377e395143f0a29aed88654bff1c95", "27a285fe87f1d0afb44f2310824f49bbf1aaea02b856d314412119142ecfbb46ece7dcadc6c516c4d3918532df9375bd9f377e395143f0a29aed88654bff1c95", "jessie@gmail.com", "user", "200: Server: Get Nodes Success")]
        public async Task GetTheNodes(string userHash, string accountHash, string username, string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITreeManagementManager treeManagementManager = TestProvider.GetRequiredService<ITreeManagementManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //Act
            Tuple<Tree, string> result = await treeManagementManager.GetNodesAsync(userHash, accountHash, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result.Item2);
        }
    }
}
