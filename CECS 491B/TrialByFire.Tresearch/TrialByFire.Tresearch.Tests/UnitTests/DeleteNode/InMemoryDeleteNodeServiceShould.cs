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

namespace TrialByFire.Tresearch.Tests.UnitTests.DeleteNode
{
    public class InMemoryDeleteNodeServiceShould : TestBaseClass 
    {
        public InMemoryDeleteNodeServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IDeleteNodeService, DeleteNodeService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData(13, 12, "200: Server: Delete Node Success")]
        [InlineData(80085, 80084, "504: Database: The node was not found.")]
        public async Task DeleteTheNode(long nodeID, long parentID, string expected)
        {
            //Arrange
            IDeleteNodeService deleteNodeService = TestProvider.GetService<IDeleteNodeService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            
            //Act
            IResponse<string> result = await deleteNodeService.DeleteNodeAsync(nodeID, parentID, cancellationTokenSource.Token);

            //Assert
            //Assert.Equal(expected, result);
        }
    }
}
