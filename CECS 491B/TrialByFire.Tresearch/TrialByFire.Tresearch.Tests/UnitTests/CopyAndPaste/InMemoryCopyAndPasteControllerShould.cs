using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

namespace TrialByFire.Tresearch.Tests.UnitTests.CopyAndPaste
{
    public class InMemoryCopyAndPasteControllerShould : TestBaseClass
    {
        private ICopyAndPasteController _copyAndPasteController;
        public InMemoryCopyAndPasteControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICopyAndPasteService, CopyAndPasteService>();
            TestServices.AddScoped<ICopyAndPasteManager, CopyAndPasteManager>();
            TestServices.AddScoped<ICopyAndPasteController, CopyAndPasteController>();
            TestProvider = TestServices.BuildServiceProvider();
            _copyAndPasteController = TestProvider.GetService<ICopyAndPasteController>();


        }

        
        

        [Theory]
        [MemberData(nameof(CopyNodeData))]

        public async Task CopyNodeAsync(IRoleIdentity roleIdentity, List<long> nodesCopy, IResponse<IList<Node>> expected)
        {

            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            // Act
            ActionResult <IEnumerable<Node>> response = await _copyAndPasteController.CopyNodeAsync(nodesCopy).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Value);

        }


        
        /*
        
        [Theory]
        [MemberData(nameof(PasteNodeData))]


        public async Task PasteNodeAsync(IRoleIdentity roleIdentity, long nodeIDtoPasteTo, List<INode> nodes, IResponse<string> expected)
        {

            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            // Act
            ActionResult<string> response = await _copyAndPasteController.PasteNodeAsync(nodeIDtoPasteTo, nodes).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Value);

        }

        */
        







        //separate tests up into different tests
        public static IEnumerable<object[]> CopyNodeData()
        {


            Node node1 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 1, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 17), true, false);
            Node node2 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 2, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 18), true, false);
            Node node3 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 3, 1, "Cooking Rice", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            
            
            Node node4 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 4, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 17), true, false);
            Node node5 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 5, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 18), true, false);
            Node node6 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 6, 1, "Cooking Rice", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node7 = new Node("AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 7, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node8 = new Node("E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159", 8, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node9 = new Node("AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 9, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 19), true, false);







            // User not authenticated and nodeID list is empty
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "guest", "guest", "");
            var nodesToCopyList0 = new List<long>();
            IResponse<IList<Node>> nodesCopiedResult0 = new CopyResponse<IList<Node>>("401: Server: No active session found. Please login and try again.", null, 400, false);


            var resultCase0 = IMessageBank.Responses.notAuthenticated;


            // User authenticated but nodeID list is empty
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "grizzly@gmail.com", "user", "87ec69f0ab41c3dcb31e01dcf9942d756501b421887524a1e691dff69a698cf1d46c26b68f73dddb29a7d2729eddf43580bab9a5002d2289c0c7bf4d5db7c7ae");
            var nodesToCopyList1 = new List<long>();
            IResponse<IList<Node>> nodesCopiedResult1 = new CopyResponse<IList<Node>>("400: Server: No nodes to copy failure", null, 400, false);
            

            var resultCase1 = IMessageBank.Responses.copyNodeEmptyError;





            // User authenticated and nodeID list is populated, and was successful in grabbing nodes from database
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "grizzly@gmail.com", "user", "87ec69f0ab41c3dcb31e01dcf9942d756501b421887524a1e691dff69a698cf1d46c26b68f73dddb29a7d2729eddf43580bab9a5002d2289c0c7bf4d5db7c7ae");
            var nodesToCopyList3 = new List<long> { 1, 2, 3 };
            var nodesCopiedList3 = new List<Node> { node1, node2, node3 };
            IResponse<IList<Node>> nodesCopiedResult3 = new CopyResponse<IList<Node>>("200: Server: Copy Node Successful", nodesCopiedList3, 200, false);


            var resultCase3 = IMessageBank.Responses.copyNodeSuccess;




            return new[]
            {

                new object[] { roleIdentity0, nodesToCopyList0, nodesCopiedResult0},
                new object[] { roleIdentity1, nodesToCopyList1, nodesCopiedResult1},
                //new object[] { roleIdentity2, nodesToCopyList2, nodesCopiedResult2},
                new object[] { roleIdentity3, nodesToCopyList3, nodesCopiedResult3},

            };
        }



        





        
        /*
        public static IEnumerable<object[]> PasteNodeData()
        {
            return new[]
            {
                new object[]{ }
            };
        }
        
        */


        


    }
}
