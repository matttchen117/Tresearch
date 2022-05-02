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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.CopyAndPaste
{
    public class CopyAndPasteManagerShould : TestBaseClass
    {
        private ICopyAndPasteManager _copyAndPasteManager;

        public CopyAndPasteManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ICopyAndPasteService, CopyAndPasteService>();
            TestServices.AddScoped<ICopyAndPasteManager, CopyAndPasteManager>();
            TestProvider = TestServices.BuildServiceProvider();
            _copyAndPasteManager = TestProvider.GetService<ICopyAndPasteManager>();
        }


        [Theory]
        [MemberData(nameof(CopyNodeData))]

        public async Task CopyNodeAsync(IRoleIdentity roleIdentity, List<long> nodesCopy, IResponse<IList<Node>> expected)
        {
            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            // Act
            IResponse<IEnumerable<Node>> response = await _copyAndPasteManager.CopyNodeAsync(nodesCopy).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Data);


        }



        public static IEnumerable<object[]> CopyNodeData()
        {


            Node node1 = new Node(1, 1, "Computer Programming", "Breif overview of computer programming", true, false, "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", 1);
            Node node2 = new Node(2, 1, "Intro to HTML/CSS", "An annoying language", true, false, "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", 1);
            Node node3 = new Node(3, 1, "Containers", "Must knows", true, false, "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75", 1);


            //Node node32 = new Node(32, 27, "Test Node", "Testing", true, false, "75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6", 1);





            // User not authenticated and nodeID list is empty
            //IRoleIdentity identity = Thread.CurrentPrincipal.Identity;
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "guest", "guest", "");
            var nodesToCopyList0 = new List<long>();
            IResponse<IList<Node>> nodesCopiedResult0 = new CopyResponse<IList<Node>>("401: Server: No active session found. Please login and try again.", null, 400, false);

            // User authenticated but nodeID list is empty
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "grizzly@gmail.com", "user", "87ec69f0ab41c3dcb31e01dcf9942d756501b421887524a1e691dff69a698cf1d46c26b68f73dddb29a7d2729eddf43580bab9a5002d2289c0c7bf4d5db7c7ae");
            var nodesToCopyList1 = new List<long>();
            IResponse<IList<Node>> nodesCopiedResult1 = new CopyResponse<IList<Node>>("400: Server: No nodes to copy failure", null, 400, false);





            // User authenticated and nodeID list is populated, and was successful in grabbing nodes from database
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "viet.nguyen03@student.csulb.edu", "user", "75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6");
            var nodesToCopyList3 = new List<long> { 1 };
            var nodesCopiedList3 = new List<Node> { node1 };
            IResponse<IList<Node>> nodesCopiedResult3 = new CopyResponse<IList<Node>>("", nodesCopiedList3, 200, true);


            // User authenticated and copying multiple nodes, successfully copied all nodes
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "viet.nguyen03@student.csulb.edu", "user", "75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6");
            var nodesToCopyList4 = new List<long> { 1, 2, 3 };
            var nodesCopiedList4 = new List<Node> { node1, node2, node3 };
            IResponse<IList<Node>> nodesCopiedResult4 = new CopyResponse<IList<Node>>("", nodesCopiedList4, 200, true);





            return new[]
            {
                new object[] { roleIdentity0, nodesToCopyList0, nodesCopiedResult0},
                new object[] { roleIdentity1, nodesToCopyList1, nodesCopiedResult1},
                new object[] { roleIdentity3, nodesToCopyList3, nodesCopiedResult3},
                new object[] { roleIdentity4, nodesToCopyList4, nodesCopiedResult4},

            };
        }
    }
}
