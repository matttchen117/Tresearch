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

namespace TrialByFire.Tresearch.Tests.UnitTests.CreateNode
{
    public class InMemoryCreateNodeServiceShould : TestBaseClass
    {
        public InMemoryCreateNodeServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            14, "Deadlift", "Deadlift for ORM", true, false, "200: Server: Create Node Success")]
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
