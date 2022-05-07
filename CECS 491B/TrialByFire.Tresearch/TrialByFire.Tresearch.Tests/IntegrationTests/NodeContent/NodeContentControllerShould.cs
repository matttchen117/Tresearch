using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.NodeContent
{
    public class NodeContentControllerShould : TestBaseClass
    {
        private INodeContentController _nodeContentController;
        private IMessageBank _messageBank;
        public NodeContentControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<INodeContentService, NodeContentService>();
            TestServices.AddScoped<INodeContentManager, NodeContentManager>();
            TestServices.AddScoped<INodeContentController, NodeContentController>();
            TestProvider = TestServices.BuildServiceProvider();
            _nodeContentController = TestProvider.GetService<INodeContentController>();
            _messageBank = TestProvider.GetService<IMessageBank>();
        }

        [Theory]
        [MemberData(nameof(ChangeNodeContentInputData))]
        public async Task UpdateTheNodeContentAsync(string owner, long nodeID, string title, string content, 
            IRolePrincipal principal, ObjectResult expected)
        {
            // Arrange
            Enum.TryParse(expected.Value.ToString(), out IMessageBank.Responses data);
            expected.Value = await _messageBank.GetMessage(data).ConfigureAwait(false);
            Thread.CurrentPrincipal = principal;

            // Act
            IActionResult response = await _nodeContentController.UpdateNodeContentAsync(owner, 
                nodeID, title, content).ConfigureAwait(false);
            var objectResult = response as ObjectResult;

            // Assert
            Assert.Equal(expected.StatusCode, objectResult.StatusCode);
            Assert.Equal(expected.Value, objectResult.Value);
        }

        public static IEnumerable<object[]> ChangeNodeContentInputData()
        {

            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "drakat7@gmail.com", "user",
                "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6");
            IRolePrincipal rolePrincipal1 = new RolePrincipal(roleIdentity1);
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "guest", "guest",
                    "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");
            IRolePrincipal rolePrincipal2 = new RolePrincipal(roleIdentity2);

            return new[]
            {
                new object[] {"0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    13, "Test Complete", "Test successful.", rolePrincipal1,
                    new ObjectResult("updateNodeContentSuccess") {StatusCode = 200 } },
                new object[] {"0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    13, "Test Complete", "Test successful.", rolePrincipal2,
                    new ObjectResult("notAuthorized") {StatusCode = 400 } },
            };
        }
    }
}
