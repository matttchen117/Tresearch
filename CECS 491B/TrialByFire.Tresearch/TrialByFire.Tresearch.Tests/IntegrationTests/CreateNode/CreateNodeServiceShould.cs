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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.CreateNode
{
    public class CreateNodeServiceShould : TestBaseClass
    {
        public CreateNodeServiceShould() : base(new string[] { })
        {
            //TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("jessie@gmail.com", 69420, 69419, "Cooking", "Concepts of Preparing Food", true, "jessie@gmail.com", "200: Server: success")]
        [InlineData("larry@gmail.com", 100000, 100001, "Title 1", "Summary 1", false, "larry@gmail.com", "409: Database: Node Already Exists")]
        public async Task CreateTheNode(string username, long nodeID, long parentID, string nodeTitle, string summary, bool mode,
            string accountOwner, string expected)
        {
            //Arrange
            ICreateNodeService createNodeService = TestProvider.GetRequiredService<ICreateNodeService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(nodeID, parentID, nodeTitle, summary, mode, accountOwner);

            //Act
            string result = await createNodeService.CreateNodeAsync(username, node, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
