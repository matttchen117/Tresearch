using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.NodeContent
{
    public class NodeContentManagerShould : TestBaseClass
    {
        private INodeContentManager _nodeContentManager;
        private IMessageBank _messageBank;
        public NodeContentManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<INodeContentService, NodeContentService>();
            TestServices.AddScoped<INodeContentManager, NodeContentManager>();
            TestProvider = TestServices.BuildServiceProvider();
            _nodeContentManager = TestProvider.GetService<INodeContentManager>();
            _messageBank = TestProvider.GetService<IMessageBank>();
        }

        [Theory]
        [MemberData(nameof(ChangeNodeContentInputData))]
        public async Task UpdateTheNodeContentAsync(INodeContentInput nodeContentInput, IRolePrincipal principal,
            IResponse<string> expected)
        {
            // Arrange
            if(!expected.ErrorMessage.Equals(""))
            {
                Enum.TryParse(expected.ErrorMessage, out IMessageBank.Responses message);
                expected.ErrorMessage = await _messageBank.GetMessage(message).ConfigureAwait(false);
            }
            if (expected.Data != null && !expected.Data.Equals(""))
            {
                Enum.TryParse(expected.Data, out IMessageBank.Responses data);
                expected.Data = await _messageBank.GetMessage(data).ConfigureAwait(false);
            }
            Thread.CurrentPrincipal = principal;

            // Act
            IResponse<string> response = await _nodeContentManager.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.ErrorMessage, response.ErrorMessage);
            Assert.Equal(expected.Data, response.Data);
        }

        [Theory]
        [MemberData(nameof(ChangeNodeContentInputData))]
        public async Task UpdateTheNodeContentAsyncWithin5Seconds(INodeContentInput nodeContentInput, IRolePrincipal principal,
            IResponse<string> expected)
        {
            // Arrange
            if (!expected.ErrorMessage.Equals(""))
            {
                Enum.TryParse(expected.ErrorMessage, out IMessageBank.Responses message);
                expected.ErrorMessage = await _messageBank.GetMessage(message).ConfigureAwait(false);
            }
            if (expected.Data != null && !expected.Data.Equals(""))
            {
                Enum.TryParse(expected.Data, out IMessageBank.Responses data);
                expected.Data = await _messageBank.GetMessage(data).ConfigureAwait(false);
            }
            Thread.CurrentPrincipal = principal;
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));
            nodeContentInput.CancellationToken = cancellationTokenSource.Token;

            // Act
            IResponse<string> response = await _nodeContentManager.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.ErrorMessage, response.ErrorMessage);
            Assert.Equal(expected.Data, response.Data);
        }

        public static IEnumerable<object[]> ChangeNodeContentInputData()
        {
            INodeContentInput nodeContentInput1 = new NodeContentInput(
                    "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    13, "Test Complete", "Test successful.");
            INodeContentInput nodeContentInput2 = new NodeContentInput(
                    "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    13, "Test Complete", "Test successful.");

            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "drakat7@gmail.com", "user",
                "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6");
            IRolePrincipal rolePrincipal1 = new RolePrincipal(roleIdentity1);
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "guest", "guest",
                    "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            IRolePrincipal rolePrincipal2 = new RolePrincipal(roleIdentity2);

            return new[]
            {
                new object[] {nodeContentInput1, rolePrincipal1,
                        new NodeContentResponse<string>("", "updateNodeContentSuccess", 200, true) },
                new object[] {nodeContentInput2, rolePrincipal2,
                        new NodeContentResponse<string>("notAuthorized", "", 400, false) },
            };
        }
    }
}
