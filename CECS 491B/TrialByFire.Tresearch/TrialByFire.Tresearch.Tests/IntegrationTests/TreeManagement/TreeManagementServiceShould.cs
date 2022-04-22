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
        [InlineData("AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", "200: Server: Get Nodes Success")]
        public async Task GetTheNodes(string userhash, string expected)
        {
            //Arrange
            ITreeManagementService treeManagementService = TestProvider.GetRequiredService<ITreeManagementService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            
            //Act
            Tuple<Tree, string> result = await treeManagementService.GetNodesAsync(userhash, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result.Item2);
        }
    }
}
