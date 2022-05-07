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
        [InlineData("75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6", 
            1702, "Deadl", "Deadlift for ORM", true, false, "200: Server: Create Node Success")]
        public async Task CreateTheNode(string userhash, long parentID, string nodeTitle, string summary, bool visibility, bool deleted,
            string expected)
        {
            // Arrange
            ICreateNodeService createNodeService = TestProvider.GetRequiredService<ICreateNodeService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(userhash, 0, parentID, nodeTitle, summary, DateTime.UtcNow, visibility, deleted);

            // Act
            IResponse<string> response = await createNodeService.CreateNodeAsync(node, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, response.Data);
        }
    }
}
