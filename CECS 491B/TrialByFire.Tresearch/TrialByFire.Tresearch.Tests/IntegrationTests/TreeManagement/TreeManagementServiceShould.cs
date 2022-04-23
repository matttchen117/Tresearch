using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.TreeManagement
{
    public class TreeManagementServiceShould : TestBaseClass
    {
        public TreeManagementServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ITreeManagementService, TreeManagementService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", "27a285fe87f1d0afb44f2310824f49bbf1aaea02b856d314412119142ecfbb46ece7dcadc6c516c4d3918532df9375bd9f377e395143f0a29aed88654bff1c95", "200: Server: Get Nodes Success")]
        public async Task GetTheNodes(string userhash, string accountHash, string expected)
        {
            //Arrange
            ITreeManagementService treeManagementService = TestProvider.GetRequiredService<ITreeManagementService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            
            //Act
            Tuple<Tree, string> result = await treeManagementService.GetNodesAsync(userhash, accountHash, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result.Item2);
        }
    }
}
