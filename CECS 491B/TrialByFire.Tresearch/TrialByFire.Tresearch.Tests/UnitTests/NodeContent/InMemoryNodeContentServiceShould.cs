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
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.NodeContent
{
    public class InMemoryNodeContentServiceShould : TestBaseClass
    {
        private INodeContentService _nodeContentService;
        private IMessageBank _messageBank;
        public InMemoryNodeContentServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<INodeContentService, NodeContentService>();
            TestProvider = TestServices.BuildServiceProvider();
            _nodeContentService = TestProvider.GetService<INodeContentService>();
            _messageBank = TestProvider.GetService<IMessageBank>();
        }

        [Theory]
        [MemberData(nameof(ChangeNodeContentInputData))]
        public async Task UpdateTheNodeContentAsync(INodeContentInput nodeContentInput, IResponse<string> expected)
        {
            // Arrange
            Enum.TryParse(expected.Data, out IMessageBank.Responses data);
            expected.Data = await _messageBank.GetMessage(data).ConfigureAwait(false);

            // Act
            IResponse<string> response = await _nodeContentService.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.ErrorMessage, response.ErrorMessage);
            Assert.Equal(expected.Data, response.Data);
        }

        public static IEnumerable<object[]> ChangeNodeContentInputData()
        {
            INodeContentInput nodeContentInput1 = new NodeContentInput(
                    "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    13, "Test Complete", "Test successful.");
            return new[]
            {
                new object[] {nodeContentInput1,
                        new NodeContentResponse<string>("", "updateNodeContentSuccess", 200, true) },
            };
        }
    }
}
