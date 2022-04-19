using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{

    /// <summary>
    ///     Tests the Controller Layer of Tag Feature.
    /// </summary>
    public class TagControllerShould: TestBaseClass, IClassFixture<TagControllerDatabaseFixture>
    {
        public static TagControllerDatabaseFixture fixture { get; set; }    // Access to the fixture instance (this is where data to database is initialized and  disposed)

        private static List<long> nodeids = new List<long>();               // Holds the ID of Nodes (need to have since nodes are dynamically created (NodeID is Identity)
        private static List<long> NodeIDs                                   // Getter and setter for static list of node ids
        {
            get { return nodeids; }
            set { nodeids = value; }
        }

        public TagControllerShould(TagControllerDatabaseFixture fixture) : base(new string[] { })
        {
            TagControllerShould.fixture = fixture;                                                  
            NodeIDs = fixture.nodeIDs;
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<IAccountVerificationService, AccountVerificationService>();
            TestServices.AddScoped<ITagManager, TagManager>();
            TestServices.AddScoped<ILogManager, LogManager>();
            TestServices.AddScoped<ITagController, TagController>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        /**
         *  Case 0: Tag nodes. User is Authenticated and Authorized. Nodes exist and already contain tag. 
         *      Status code: 200 Value: Tag added to node(s).
         *  Case 1: Tag nodes. User is Authenticated and Authorized. Nodes exist and do not contain tag.
         *      Status code: 200 Value: Tag added to node(s).
         *  Case 2: Tag nodes. User is Authenticated and Authorized. Node exists and contains tag
         *      Status code: 200 Value: Tag added to node(s).
         *  Case 3: Tag nodes. User is Authenticated and Authorized. Node exists and does not contain tag
         *      Status code: 200 Value: Tag added to node(s).
         *  Case 4: Tag nodes. User is Authenticated. No node passed in
         *      Status code: 404 Value: The node was not found.
         *  Case 5: Tag nodes. User is Authenticated and Authorized. Node exists but tag doesn't exist
         *     Status code: 404 Value: The tag was not found.
         *  Case 6: Tagging node. Some nodes already contain tag.
         *      Status code: 200 Value: Tag added to node(s).
         *  Case 7: Tag nodes. User is authenticated but not authorized to make changes to ALL nodes.
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 8: Tag nodes. User is authenticatd but not authorized to make changes to SOME nodes.    
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 9: Tag nodes. User is authenticatd but not authorized to make changes to node.
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 10: Tag nodes. User is not authenticated
         *      Status code: 401 Value: No active session found. Please login and try again.
         *  Case 11: Tag nodes. User is not enabled
         *      Status code: 401 Value: UserAccount disabled. Perform account recovery or contact system admin.
         *  Case 12: Tag nodes. Account does not exist
         *      Status code: 404 Value: Account not found.
         */
        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            //Act
            IActionResult aResult = await tagController.AddTagToNodesAsync(GetNodes(index), tagName);
            var result = aResult as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }


        /**
         *  Case 0: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and already contain tag. 
         *      Status code: 200 Value: Tag removed from node(s).
         *  Case 1: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and do not contain tag.
         *      Status code: 200 Value: Tag removed from node(s).
         *  Case 2: Remove nodes tags. User is Authenticated and Authorized. Node exists and contains tag
         *      Status code: 200 Value: Tag removed from node(s).
         *  Case 3: Remove nodes tags. User is Authenticated and Authorized. Node exists and does not contain tag
         *      Status code: 200 Value: Tag removed from node(s).
         *  Case 4: Remove nodes tags. User is Authenticated. No node passed in
         *      Status code: 404 Value: The node was not found.
         *  Case 5: Remove nodes tags. User is Authenticated and Authorized. Node exists but tag doesn't exist
         *     Status code: 200 Value:Tag removed from node(s).
         *  Case 6: Remove nodes tags. Some nodes already contain tag.
         *      Status code: 200 Value: Tag removed from node(s).
         *  Case 7: Remove nodes tags. User is authenticated but not authorized to make changes to ALL nodes.
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 8: Remove nodes tags. User is authenticatd but not authorized to make changes to SOME nodes.    
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 9: Remove nodes tags. User is authenticatd but not authorized to make changes to node.
         *      Status code: 403 Value: You are not authorized to perform this operation.
         *  Case 10: Remove nodes tags. User is not authenticated
         *      Status code: 401 Value: No active session found. Please login and try again.
         *  Case 11: Remove nodes tags. User is not enabled
         *      Status code: 401 Value: UserAccount disabled. Perform account recovery or contact system admin.
         *  Case 12: Remove nodes tags. Account does not exist
         *      Status code: 404 Value: Account not found.
         */
        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveNodTagData(IRoleIdentity roleIdentity, List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            //Act
            IActionResult aResult = await tagController.RemoveTagFromNodesAsync(GetNodes(index), tagName);
            var result = aResult as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        

        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(IRoleIdentity roleIdentity, List<int> index, List<string> expectedTags, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            ITagController tagController = TestProvider.GetService<ITagController>();

            //Act
            IActionResult results = await tagController.GetNodeTagsAsync(GetNodes(index));
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        }

        public static IEnumerable<object[]> GetNodeTagData()
        {
            /*User Authorized, Nodes contain shared tags but also have other tags
             *          9019303356: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *          9019303357: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *          9019303358: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             */

            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase0 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase0 = new List<int> { 6, 7, 8 };
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /*User Authorized, Nodes do not contain shared tags
             *          9019303358: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             *          9019303359: Tresearch Controller Get Tag2
             *          9019303360: Tresearch Controller Get Tag1
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase1 = new List<string> { };
            var nodeListCase1 = new List<int> { 8, 9, 10 };
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Grab Single Node's Tags (9019303356: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2)
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase2 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase2 = new List<int> { 6 };
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Node exists but does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase3 = new List<string> { };
            var nodeListCase3 = new List<int> { 11 };
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase4 = new List<string> { };
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;


            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase5 = new List<string> { };
            var nodeListCase5 = new List<int> { 0, 1, 2 };
            var resultCase5 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 9019303353, 9019303354 but not 9019303350)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase6 = new List<string> { };
            var nodeListCase6 = new List<int> { 0, 3, 4 };
            var resultCase6 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase7 = new List<string> { };
            var nodeListCase7 = new List<int> { 0 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "guest", "guest", "");
            var tagListCase8 = new List<string> { };
            var nodeListCase8 = new List<int> { 0 };
            var resultCase8 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity9 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            var tagListCase9 = new List<string> { };
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity10 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            var tagListCase10 = new List<string> { };
            var nodeListCase10 = new List<int> { 6 };
            var resultCase10 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "user");
            var tagListCase11 = new List<string> { };
            var nodeListCase11 = new List<int> { };
            var resultCase11 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { roleIdentity0, nodeListCase0, tagListCase0, resultCase0 },
                new object[] { roleIdentity1, nodeListCase1, tagListCase1, resultCase1 },
                new object[] { roleIdentity2, nodeListCase2, tagListCase2, resultCase2 },
                new object[] { roleIdentity3, nodeListCase3, tagListCase3, resultCase3 },
                new object[] { roleIdentity4, nodeListCase4, tagListCase4, resultCase4 },
                new object[] { roleIdentity5, nodeListCase5, tagListCase5, resultCase5 },
                new object[] { roleIdentity6, nodeListCase6, tagListCase6, resultCase6 },
                new object[] { roleIdentity7, nodeListCase7, tagListCase7, resultCase7 },
                new object[] { roleIdentity8, nodeListCase8, tagListCase8, resultCase8 },
                new object[] { roleIdentity9, nodeListCase9, tagListCase9, resultCase9 },
                new object[] { roleIdentity10, nodeListCase10, tagListCase10, resultCase10 },
                new object[] { roleIdentity11, nodeListCase11, tagListCase11, resultCase11 }
            };
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTag(IRoleIdentity roleIdentity, string tagName, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            ITagController tagController = TestProvider.GetService<ITagController>();

            //Act
            IActionResult results = await tagController.CreateTagAsync(tagName);
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, expectedResult.Value);
        }

        public static IEnumerable<object[]> CreateTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName0 = "Tresearch Controller Doesnt Exist";
            string resultCase0 = "200: Server: Tag created in tag bank.";

            //User is Authenticated and Authorized as admin, tag exists
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName1 = "Tresearch Controller Tag Exist";
            string resultCase1 = "409: Database: The tag already exists.";

            //User is Authenticated but not authorized (user)
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            string tagName2 = "Tresearch Controller Doesnt Managaer";
            string resultCase2 = "403: Database: You are not authorized to perform this operation.";

            //User is not Authenticated
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            string tagName3 = "Tresearch Controller Doesnt Matter";
            string resultCase3 = "401: Server: No active session found. Please login and try again.";


            //UserAccount  does not exist
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            string tagName4 = "Tresearch Controller Doesnt Matter";
            var resultCase4 = "500: Database: The UserAccount was not found.";

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 }
            };
        }

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            ITagController tagController = TestProvider.GetService<ITagController>();

            //Act
            IActionResult results = await tagController.DeleteTagAsync(tagName);
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        public static IEnumerable<object[]> DeleteTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName0 = "Tresearch Controller REMOVE Doesnt Exist";
            string resultCase0 = "200: Server: Tag removed from tag bank.";

            //User is Authenticated and Authorized as admin, tag exists
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName1 = "Tresearch Controller REMOVE Tag Exist";
            string resultCase1 = "200: Server: Tag removed from tag bank.";

            //User is Authenticated but not authorized (user)
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            string tagName2 = "Tresearch Controller Doesnt Matter";
            string resultCase2 = "403: Database: You are not authorized to perform this operation.";

            //User is not Authenticated
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            string tagName3 = "Tresearch Controller Doesnt Matter";
            string resultCase3 = "401: Server: No active session found. Please login and try again.";


            //UserAccount  does not exist
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            string tagName4 = "Tresearch Controller Doesnt Matter";
            var resultCase4 = "500: Database: The UserAccount was not found.";

            //User is Authenticated and Authorized as admin, tag exists and other nodes have this tag
            IRoleIdentity roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName5 = "Tresearch Controller REMOVE Exist and Tagged";
            string resultCase5 = "200: Server: Tag removed from tag bank.";

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 },
                new object[] { roleIdentity5, tagName5, resultCase5 }
            };
        }

        [Theory]
        [MemberData(nameof(GetTagData))]
        public async Task GetTagsAsync(IRoleIdentity roleIdentity, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            ITagController tagController = TestProvider.GetService<ITagController>();

            //Act
            IActionResult resultTags = await tagController.GetTagsAsync();
            var result = resultTags as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        }

        public static IEnumerable<object[]> AddNodeTagData()
        {
            /**
             *  Case 0: Tag nodes. User is Authenticated and Authorized. Nodes exist and already contain tag.
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase0 = "Tresearch Controller Add Tag1";
            var nodeListCase0 = new List<int> { 0, 1, 2 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 1: Tag nodes. User is Authenticated and Authorized. Nodes exist and do not contain tag.
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase1 = "Tresearch Controller Add Tag2";
            var nodeListCase1 = new List<int> { 0, 1, 2 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 2: Tag nodes. User is Authenticated and Authorized. Node exists and contains tag
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase2 = "Tresearch Controller Add Tag3";
            var nodeListCase2 = new List<int> { 0 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 3: Tag nodes. User is Authenticated and Authorized. Node exists and does not contain tag
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Add Tag4 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase3 = "Tresearch Controller Add Tag4";
            var nodeListCase3 = new List<int> { 0 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tag nodes. User is Authenticated. No node passed in
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    
             *      Tag Name:                   Tresearch Manager Add Tag4 
             *      
             *      Result:                     "404: Database: The node was not found."
             */
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase4 = "Tresearch Controller Add Tag4";
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            /**
              *  Case 5: Tag nodes. User is Authenticated and Authorized. Node exists but tag doesn't exist
              *      Account:                    tagControllerIntegration1@tresearch.system
              *      NodeIDs:                    xxxx0
              *      Tag Name:                   Tresearch Manager Add Tag5 
              *      
              *      Result:                     "404: Database: Tag not found."
              */
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase5 = "Tresearch Controller Add Tag5";
            var nodeListCase5 = new List<int> { 0 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            /**
             *  Case 6: Tag nodes. User is Authenticated and Authorized. Some nodes already have tag 
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 (xxxx1 contains tag alredy)
             *      Tag Name:                   Tresearch Manager Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase6 = "Tresearch Controller Add Tag3";
            var nodeListCase6 = new List<int> { 0, 1, 2 };
            var resultCase6 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 7: Tag nodes. User is authenticated but not authorized to make changes to ALL nodes.
             *      Account:                    tagControllerIntegration2@tresearch.system (not authorized to make changes to all three nodes)
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase7 = "Tresearch Controller Add Tag3";
            var nodeListCase7 = new List<int> { 0, 1, 2 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            /**
              *  Case 8: Tag nodes. User is authenticatd but not authorized to make changes to SOME nodes.
              *      Account:                    tagControllerIntegration2@tresearch.system
              *      NodeIDs:                    xxxx0, xxxx3, xxx4      
              *                                  xxxx0 : Not authorized
              *                                  xxxx3 : Authorized
              *                                  xxxx4 : Authorized
              *                                  
              *      Tag Name:                   Tresearch Manager Add Tag3 
              *      
              *      Result:                     403: Database: You are not authorized to perform this operation.
              */
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase8 = "Tresearch Controller Add Tag3";
            var nodeListCase8 = new List<int> { 0, 3, 4 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 9: Tag nodes. User is authenticatd but not authorized to make changes to node.
             *      Account:                    tagControllerIntegration2@tresearch.system
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase9 = "Tresearch Controller Add Tag3";
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            /**
            *  Case 10: Tag nodes. User is not authenticated
            *      Account:         
            *      NodeIDs:                    xxxx0     
            *                                  xxxx0 : Not authorized
            *                                  
            *      Tag Name:                   Tresearch Manager Add Tag3 
            *      
            *      Result:                     401: Server: No active session found. Please login and try again.
            */
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Controller Add Tag3";
            var nodeListCase10 = new List<int> { 0 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            /**
             *  Case 11: Tag nodes. User is not enabled
             *      Account:                    tagControllerIntegrationNotEnabled@tresearch.system
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized since disabled
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     401: Database: UserAccount disabled. Perform account recovery or contact system admin.
             */
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagControllerIntegrationNotEnabled@tresearch.system", "user", "3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee");
            var tagNameCase11 = "Tresearch Manager Add Tag3";
            var nodeListCase11 = new List<int> { 0 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            /**
             *  Case 12: Tag nodes. Account not confirmed.
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     401: Database: Account not confirmed.
             */
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagControllerIntegrationNotConfirmed@tresearch.system", "user", "89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c");
            var tagNameCase12 = "Tresearch Controller Add Tag3";
            var nodeListCase12 = new List<int> { 0 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            /**
             *  Case 13: Tag nodes. Account does not exist
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     401: Database: UserAccount disabled. Perform account recovery or contact system admin.
             */
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagControllerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Controller Add Tag3";
            var nodeListCase13 = new List<int> { 0 };
            var resultCase13 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { roleIdentity0, nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { roleIdentity1, nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { roleIdentity2, nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { roleIdentity3, nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { roleIdentity4, nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { roleIdentity5, nodeListCase5, tagNameCase5, resultCase5 },
                new object[] { roleIdentity6, nodeListCase6, tagNameCase6, resultCase6 },
                new object[] { roleIdentity7, nodeListCase7, tagNameCase7, resultCase7 },
                new object[] { roleIdentity8, nodeListCase8, tagNameCase8, resultCase8 },
                new object[] { roleIdentity9, nodeListCase9, tagNameCase9, resultCase9 },
                new object[] { roleIdentity10, nodeListCase10, tagNameCase10, resultCase10 },
                new object[] { roleIdentity11, nodeListCase11, tagNameCase11, resultCase11 },
                new object[] { roleIdentity12, nodeListCase12, tagNameCase12, resultCase12 },
                new object[] { roleIdentity13, nodeListCase13, tagNameCase13, resultCase13 }
            };
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            /**
             *  Case 0: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and already contain tag. 
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Delete Tag1 
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase0 = "Tresearch Controller Delete Tag1";
            var nodeListCase0 = new List<int> { 0, 1, 2 };
            var resultCase0 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 1: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and do not contain tag. 
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Delete Tag2 
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase1 = "Tresearch Controller Delete Tag2";
            var nodeListCase1 = new List<int> { 0, 1, 2 };
            var resultCase1 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 2: Remove nodes tags. User is Authenticated and Authorized. Node exists and contains tag
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Delete Tag3 
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase2 = "Tresearch Controller Delete Tag3";
            var nodeListCase2 = new List<int> { 0 };
            var resultCase2 = IMessageBank.Responses.tagRemoveSuccess;

            /**
            *  Case 3: Remove nodes tags. User is Authenticated and Authorized. Node exists and does not contain tag
            *      NodeIDs:                    xxxx0
            *      Tag Name:                   Tresearch Manager Delete Tag4 
            *      
            *      Result:                     200: Server: Tag removed from node(s).
            */
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase3 = "Tresearch Controller Delete Tag4";
            var nodeListCase3 = new List<int> { 0 };
            var resultCase3 = IMessageBank.Responses.tagRemoveSuccess;

            /**
            *  Case 4: Remove nodes tags. No node passed in
            *      NodeIDs:                    
            *      Tag Name:                   Tresearch Service Delete Tag4
            *      
            *      Result:                     404: Database: The node was not found.
            */
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase4 = "Tresearch Controller Delete Tag4";
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 5: Remove nodes tags. User is Authenticated and Authorized. Node exists but tag doesn't exist
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Delete Tag5 
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase5 = "Tresearch Controller Delete Tag5";
            var nodeListCase5 = new List<int> { 0 };
            var resultCase5 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 6: Remove nodes tags. User is Authenticated and Authorized. Some nodes already have tag 
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 (xxxx1 contains tag alredy)
             *      Tag Name:                   Tresearch Manager Delete Tag2 
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase6 = "Tresearch Controller Delete Tag3";
            var nodeListCase6 = new List<int> { 0, 1, 2 };
            var resultCase6 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 7: Remove nodes tags. User is authenticated but not authorized to make changes to ALL nodes.
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 
             *      Tag Name:                   Tresearch Manager Delete Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase7 = "Tresearch Controller Delete Tag3";
            var nodeListCase7 = new List<int> { 0, 1, 2 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 8: Remove nodes tags. User is authenticatd but not authorized to make changes to SOME nodes.
             *      NodeIDs:                    xxxx0, xxxx3, xxx4      
             *                                  xxxx0 : Not authorized
             *                                  xxxx3 : Authorized
             *                                  xxxx4 : Authorized
             *                                  
             *      Tag Name:                   Tresearch Delete Delete Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase8 = "Tresearch Controller Delete Tag3";
            var nodeListCase8 = new List<int> { 0, 3, 4 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 9: Remove nodes tags. User is authenticatd but not authorized to make changes to node.
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Delete Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase9 = "Tresearch Controller Delete Tag3";
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            /**
              *  Case 10: Remove nodes tags. User is not authenticated
              *      NodeIDs:                    xxxx0     
              *                                  
              *      Tag Name:                   Tresearch Manager Delete Tag3 
              *      
              *      Result:                     401: Server: No active session found. Please login and try again.
              */
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Controller Delete Tag3";
            var nodeListCase10 = new List<int> { 0 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            /**
            *  Case 11: Remove nodes tags. User is not enabled
            *      NodeIDs:                    xxxx0     
            *                                  
            *      Tag Name:                   Tresearch Manager Delete Tag3 
            *      
            *      Result:                     401: Database: UserAccount disabled. Perform account recovery or contact system admin.
            */
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagControllerIntegrationNotEnabled@tresearch.system", "user", "3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee");
            var tagNameCase11 = "Tresearch Controller Delete Tag3";
            var nodeListCase11 = new List<int> { 0 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            /**
             *  Case 12: Tag nodes. Account not confirmed
             *      NodeIDs:                    xxxx0     
             *                                  
             *      Tag Name:                   Tresearch Manager Delete Tag3 
             *      
             *      Result:                     401: Database: Please confirm your account before attempting to login.
             */
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagControllerIntegrationNotConfirmed@tresearch.system", "user", "89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c");
            var tagNameCase12 = "Tresearch Controller Delete Tag3";
            var nodeListCase12 = new List<int> { 0 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            /**
             *  Case 13: Tag nodes. Account does not exist
             *      NodeIDs:                    xxxx0     
             *                                  
             *      Tag Name:                   Tresearch Manager Delete Tag3 
             *      
             *      Result:                     404: Database: The UserAccount not found.
             */
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagControllerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Controller Delete Tag3";
            var nodeListCase13 = new List<int> { };
            var resultCase13 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { roleIdentity0, nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { roleIdentity1, nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { roleIdentity2, nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { roleIdentity3, nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { roleIdentity4, nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { roleIdentity5, nodeListCase5, tagNameCase5, resultCase5 },
                new object[] { roleIdentity6, nodeListCase6, tagNameCase6, resultCase6 },
                new object[] { roleIdentity7, nodeListCase7, tagNameCase7, resultCase7 },
                new object[] { roleIdentity8, nodeListCase8, tagNameCase8, resultCase8 },
                new object[] { roleIdentity9, nodeListCase9, tagNameCase9, resultCase9 },
                new object[] { roleIdentity10, nodeListCase10, tagNameCase10, resultCase10 },
                new object[] { roleIdentity11, nodeListCase11, tagNameCase11, resultCase11 },
                new object[] { roleIdentity12, nodeListCase12, tagNameCase12, resultCase12 },
                new object[] { roleIdentity13, nodeListCase13, tagNameCase13, resultCase13 }
            };
        }

        public static IEnumerable<object[]> GetTagData()
        {
            //User is Authenticated and Authorized 
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagControllerIntegration1@tresearch.system", "user");
            string resultCase0 = "200: Server: Tag(s) retrieved.";

            //User is Authenticated and Authorized and Admin
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string resultCase1 = "200: Server: Tag(s) retrieved.";

            //User has not been authenticated
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "guest", "guest");
            string resultCase2 = "401: Server: No active session found. Please login and try again.";

            //User is not enabled
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            string resultCase3 = "401: Database: UserAccount disabled. Perform account recovery or contact system admin.";

            //User is not confirmed
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            string resultCase4 = "401: Database: Please confirm your account before attempting to login.";

            //User is has unknown role
            IRoleIdentity roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "wrong");
            string resultCase5 = "400: Server: Unknown role used.";

            //UserAccount not found
            IRoleIdentity roleIdentity6 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "user");
            var resultCase6 = "500: Database: The UserAccount was not found.";

            return new[]
            {
                new object[] { roleIdentity0, resultCase0 },
                new object[] { roleIdentity1, resultCase1 },
                new object[] { roleIdentity2, resultCase2 },
                new object[] { roleIdentity3, resultCase3 },
                new object[] { roleIdentity4, resultCase4 },
                new object[] { roleIdentity5, resultCase5 },
                new object[] { roleIdentity6, resultCase6 }
            };
        }

        /// <summary>
        /// Retrieves list of nodeids matching indices.
        /// </summary>
        /// <param name="indexes">Indices of return nodeIDs</param>
        /// <returns></returns>
        public List<long> GetNodes(List<int> indexes)
        {
            List<long> ids = new List<long>();
            foreach (int index in indexes)
                ids.Add(NodeIDs[index]);
            return ids;
        }

    }

    /// <summary>
    ///     Tag Controller Database Fixture Initializes the database. Runs the startup script and upon completion of testing, disposes all data created
    ///     while performing tests
    /// </summary>
    public class TagControllerDatabaseFixture : TestBaseClass, IDisposable
    {
        public List<long> nodeIDs { get; set; }
        public TagControllerDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ControllerIntegrationSetup.sql");

            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                foreach (string command in commands)
                    if (!string.IsNullOrWhiteSpace(command.Trim()))
                        using (var com = new SqlCommand(command, connection))
                            com.ExecuteNonQuery();
            }

            // Initialize list of nodes. 
            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                var procedure = "dbo.[ControllerIntegrationTagInitializeProcedure]";                        // Name of store procedure
                var value = new { };                                                                 // Parameters of stored procedure
                nodeIDs = connection.Query<long>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public void Dispose()
        {
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ControllerIntegrationCleanup.sql");

            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                foreach (string command in commands)
                    if (!string.IsNullOrWhiteSpace(command.Trim()))
                        using (var com = new SqlCommand(command, connection))
                            com.ExecuteNonQuery();
            }
        }
    }
}
