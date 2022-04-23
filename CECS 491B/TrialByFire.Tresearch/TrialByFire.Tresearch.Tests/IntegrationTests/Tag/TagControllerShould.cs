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

        /// <summary>
        ///  Constructor for tag controller layer tests
        /// </summary>
        /// <param name="fixture">Fixture class instance runs startup and cleanup scripts on database</param>
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

        /// <summary>
        ///     Tests user adding tags to node(s)
        /// </summary>
        /// <param name="roleIdentity">Identity of user</param>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();

            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            //Act
            IActionResult aResult = await tagController.AddTagToNodesAsync(nodeIDs, tagName);
            var result = aResult as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        /// <summary>
        ///     Tests user creating a tag in tag bank
        /// </summary>
        /// <param name="roleIdentity">Identity of user</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTag(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
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
            IActionResult results = await tagController.CreateTagAsync(tagName);
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, expectedResult.Value);
        }

        

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
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
            IActionResult results = await tagController.DeleteTagAsync(tagName);
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        /// <summary>
        ///     Tests users retrieving list of shared tag(s) from node(s).
        /// </summary>
        /// <param name="roleIdentity">Identity of the user</param>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="expectedTags">Expected list of shared tag(s)</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
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

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            ITagController tagController = TestProvider.GetService<ITagController>();

            //Act
            IActionResult results = await tagController.GetNodeTagsAsync(nodeIDs);
            var result = results as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedTags, result.Value);
        }

        /// <summary>
        ///     Retrieves list of tags from tag bank. User does not need to be authenticated or authorized.
        /// </summary>
        /// <param name="roleIdentity">Identity of user</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(GetTagData))]
        public async Task GetTagsAsync(IRoleIdentity roleIdentity, IMessageBank.Responses response)
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
            IActionResult resultTags = await tagController.GetTagsAsync();
            var result = resultTags as ObjectResult;

            //Arrange
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        }

        /// <summary>
        ///     Tests user removing tag from node(s)
        /// </summary>
        /// <param name="roleIdentity">Identity of user</param>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveNodeTagDataAsync(IRoleIdentity roleIdentity, List<int> index, string tagName, IMessageBank.Responses response)
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

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);


            //Act
            IActionResult aResult = await tagController.RemoveTagFromNodesAsync(nodeIDs, tagName);
            var result = aResult as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        /// <summary>
        ///     Test data to delete tag from tag bank.
        ///     <br>Case 0: Delete tag. User is Authenticated and Authorized. Tag does not exist</br>
        ///     <br>Case 1: Delete tag. User is Authenticated and Authorized. Tag exists.</br>
        ///     <br>Case 2: Delete tag. User is Authenticated but not authorized. User is not admin.</br>
        ///     <br>Case 3: Delete tag. User is not authenticated.</br>
        ///     <br>Case 4: Delete tag. Account does not exist.</br>
        ///     <br>Case 5: Delete tag. User is authenticated and authorized. Nodes have this tagged.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> DeleteTagData()
        {
            /**
            *  Case 0: Delete tag. User is Authenticated and Authorized. Tag does not exist
            *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
            *      Tag Name:                  Tresearch Controller REMOVE Doesnt Exist
            */
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Controller REMOVE Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagDeleteSuccess ;

            /**
            *  Case 1: Delete tag. User is Authenticated and Authorized. Tag exists.
            *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
            *      Tag Name:                  Tresearch Controller REMOVE Tag Exist
            */
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Controller REMOVE Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDeleteSuccess;

            /**
            *  Case 2: Delete tag. User is Authenticated but not authorized. User is not admin.
            *      Account:                   tagControllerIntegration1@tresearch.system
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Controller Doesnt Matter";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            /**
            *  Case 3: Delete tag. User is not authenticated.
            *      Account:                   guest
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Controller Doesnt Matter";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;

            /**
            *  Case 4: Delete tag. Account does not exist.
            *      Account:                   tagControllerNoAccount@tresearch.system
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Controller Doesnt Matter";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

            /**
            *  Case 5: Delete tag. User is authenticated and authorized. Nodes have this tagged.
            *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
            *      Tag Name:                  Tresearch Controller REMOVE Exist and Tagged
            */
            //User is Authenticated and Authorized as admin, tag exists and other nodes have this tag
            var roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName5 = "Tresearch Controller REMOVE Exist and Tagged";
            var resultCase5 = IMessageBank.Responses.tagDeleteSuccess;

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

        /// <summary>
        ///     Test Data for adding tag to node(s)
        ///     <br>Case 0: Tag node(s). User is Authenticated and Authorized. Nodes exist and already contain tag.</br>
        ///     <br>Case 1: Tag node(s). User is Authenticated and Authorized. Nodes exist and do not contain tag.</br>
        ///     <br>Case 2: Tag node(s). User is Authenticated and Authorized. Node exists and contains tag.</br>
        ///     <br>Case 3: Tag node(s). User is Authenticated and Authorized. Node exists and does not contain tag.</br>
        ///     <br>Case 4: Tag node(s). User is Authenticated. No node passed in.</br>
        ///     <br>Case 5: Tag node(s). User is Authenticated and Authorized. Node exists but tag doesn't exist.</br>
        ///     <br>Case 6: Tag node(s). User is Authenticated and Authorized. Some nodes already contain tag.</br>
        ///     <br>Case 7: Tag node(s). User is authenticated but not authorized to make changes to ALL nodes.</br>
        ///     <br>Case 8: Tag node(s). User is authenticatd but not authorized to make changes to SOME nodes.</br>
        ///     <br>Case 9: Tag node(s). User is authenticatd but not authorized to make changes to node.</br>
        ///     <br>Case 10: Tag node(s). User is not authenticated.</br>
        ///     <br>Case 11: Tag node(s). User is not enabled.</br>
        ///     <br>Case 12: Tag node(s). Account not confirmed. <br></br>
        ///     <br>Case 13: Tag node(s). Account does not exist.</br>
        ///     <br>Case 14: Tag node(s). User is authenticated but node list passed in is null.</br>
        ///     <br>Case 15: Tag node(s). User is Authenticated and Authorized. Tag is null.</br>
        ///     <br>Case 16: Tag node(s). User is Authenticated and Authorized. Tag is empty string</br>
        ///     <br>Case 17: Tag node(s). User is Authenticated and Authorized. Tag only contains whitespace characters.</br>
        /// </summary>
        /// <returns></returns>
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

            /**
             *  Case 14: Tag nodes. User is authenticated but node list passed in is null.
             *      NodeIDs:                         
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3           
             */
            IRoleIdentity roleIdentity14 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase14 = "Tresearch Controller Add Tag3";
            List<int> nodeListCase14 = null;
            var resultCase14 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 15: Tag nodes. User is Authenticated and Authorized. Tag is null
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   
             */
            IRoleIdentity roleIdentity15 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            string tagNameCase15 = null;
            var nodeListCase15 = new List<int> { 0, 1, 2 };
            var resultCase15 = IMessageBank.Responses.tagNameInvalid;

            /**
             *  Case 16: Tag nodes. User is Authenticated and Authorized. Tag is empty string
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   
             */
            IRoleIdentity roleIdentity16 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase16 = "";
            var nodeListCase16 = new List<int> { 0, 1, 2 };
            var resultCase16 = IMessageBank.Responses.tagNameInvalid;

            /**
             *  Case 17: Tag nodes. User is Authenticated and Authorized. Tag only contains whitespace characters.
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   
             */
            IRoleIdentity roleIdentity17 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase17 = "    ";
            var nodeListCase17 = new List<int> { 0, 1, 2 };
            var resultCase17 = IMessageBank.Responses.tagNameInvalid;


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
                new object[] { roleIdentity13, nodeListCase13, tagNameCase13, resultCase13 },
                new object[] { roleIdentity14, nodeListCase14, tagNameCase14, resultCase14 },
                new object[] { roleIdentity15, nodeListCase15, tagNameCase15, resultCase15 },
                new object[] { roleIdentity16, nodeListCase16, tagNameCase16, resultCase16 },
                new object[] { roleIdentity17, nodeListCase17, tagNameCase17, resultCase17 }
            };
        }

        /// <summary>
        ///     Test data to create tag in tag bank.
        ///     <br>Case 0: Create tag. User is Authenticated and Authorized. Tag does not exist.</br>
        ///     <br>Case 1: Create tag. User is Authenticated and Authorized. Tag already exists.</br>
        ///     <br>Case 2: Create tag. User is Authenticated but not authorized. User is not an admin.</br>
        ///     <br>Case 3: Create tag. User is Authenticated not authenticated.</br>
        ///     <br>Case 4: Create tag. Account doesn't exist.</br>
        ///     <br>Case 5: Create tag. User is authenticated and authorized. Tag is null.</br>
        ///     <br>Case 6: Create tag. User is authenticated and authorized. Tag is empty string.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> CreateTagData()
        {
            /**
             *  Case 0: Create tag. User is Authenticated and Authorized. Tag does not exist
             *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
             *      Tag Name:                  Tresearch Controller Doesnt Exist
             */
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Controller Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagCreateSuccess;

            /**
             *  Case 1: Create tag. User is Authenticated and Authorized. Tag already exists
             *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
             *      Tag Name:                  Tresearch Controller Doesnt Exist
             */
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Controller Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDuplicate;

            /**
            *  Case 2: Create tag. User is Authenticated but not authorized. User is not an admin
            *      Account:                   tagControllerIntegration1@tresearch.system
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Controller Doesnt Matter";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            /**
            *  Case 3: Create tag. User is not authenticated.
            *      Account:                   guest
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Controller Doesnt Matter";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;

            /**
            *  Case 4: Create tag. Account doesn't exist.
            *      Account:                   
            *      Tag Name:                  Tresearch Controller Doesnt Matter
            */
            var roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Controller Doesnt Matter";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

           /**
           *  Case 5: Create tag. User is authenticated and authorized. Tag is null.
           *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
           *      Tag Name:                  
           */
            var roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            string tagName5 = null;
            var resultCase5 = IMessageBank.Responses.tagNameInvalid;

            /**
           *  Case 6: Create tag. User is authenticated and authorized. Tag is empty.
           *      Account:                   tagControllerIntegrationAdmin1@tresearch.system
           *      Tag Name:                  
           */
            var roleIdentity6 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName6 = "";
            var resultCase6 = IMessageBank.Responses.tagNameInvalid;

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 },
                new object[] { roleIdentity5, tagName5, resultCase5 },
                new object[] { roleIdentity6, tagName6, resultCase6 }
            };
        }

        /// <summary>
        ///     Test data to retrieve shared tags from list of node(s).
        ///     <br>Case 0: Retrieve node(s) tag(s). User is Authenticated and Authorized. Nodes contain shared tags but also have other tags.</br>
        ///     <br>Case 1: Retrieve node(s) tag(s). User is Authenticated and Authorized. Nodes do not share shared tags.</br>
        ///     <br>Case 2: Retrieve node(s) tag(s). User is Authenticated and Authorized. Single node with tags.</br>
        ///     <br>Case 3: Retrieve node(s) tag(s). User is Authenticated and Authorized. Single node with no tags.</br>
        ///     <br>Case 4: Retrieve node(s) tag(s). User is Authenticated. No node passed in.</br>
        ///     <br>Case 5: Retrieve node(s) tag(s). User is Authenticated but not authorized for ALL nodes.</br>
        ///     <br>Case 6: Retrieve node(s) tag(s). User is Authenticated but not authorized for SOME nodes.</br>
        ///     <br>Case 7: Retrieve node(s) tag(s). User is Authenticated but not authorized for node.</br>
        ///     <br>Case 8: Retrieve node(s) tag(s). User is not authenticated.</br>
        ///     <br>Case 9: Retrieve node(s) tag(s). User is not enabled.</br>
        ///     <br>Case 10: Retrieve node(s) tag(s). User is not confirmed.</br>
        ///     <br>Case 11: Retrieve node(s) tag(s). Account does not exist.</br>
        ///     <br>Case 12: Retrieve node(s) tag(s). User is Authenticated. Node list is null.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetNodeTagData()
        {

            /**
             *  Case 0: Retrieve node(s) tag(s). User is Authenticated and Authorized. Nodes contain shared tags but also have other tags.
             *      Nodes:                     [ xxxx6, xxxx7, xxxx8]
             *                                 xxxx6: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *                                 xxxx7: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *                                 xxxx8: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             *      Result Tag List:           [ Tresearch Controller Get Tag1, Tresearch Controller Get Tag2]
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase0 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase0 = new List<int> { 6, 7, 8 };
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 1: Retrieve node(s) tag(s). User is Authenticated and Authorized. Nodes do not share shared tags.
             *      Nodes:                     [ xxxx8, xxxx9, xxx10]
             *                                 xxxx8: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             *                                 xxxx9: Tresearch Controller Get Tag2
             *                                 xxx10: Tresearch Controller Get Tag1
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase1 = new List<string> { };
            var nodeListCase1 = new List<int> { 8, 9, 10 };
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 2: Retrieve node(s) tag(s). User is Authenticated and Authorized. Single node with tags.
             *      Nodes:                     [ xxxx6]
             *                                 xxxx6: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *      Result Tag List:           [ Tresearch Controller Get Tag1, Tresearch Controller Get Tag2 ]
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase2 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase2 = new List<int> { 6 };
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 3: Retrieve node(s) tag(s). User is Authenticated and Authorized. Single node with no tags.
             *      Nodes:                     [ xxxx11]
             *                                 xxxx11: 
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase3 = new List<string> { };
            var nodeListCase3 = new List<int> { 11 };
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 4: Retrieve node(s) tag(s). User is Authenticated. No node passed in.
             *      Nodes:                     [ ]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase4 = new List<string> { };
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 5: Retrieve node(s) tag(s). User is Authenticated but not authorized for ALL nodes.
             *      Nodes:                     [ xxxx0, xxxx1, xxxx2]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase5 = new List<string> { };
            var nodeListCase5 = new List<int> { 0, 1, 2 };
            var resultCase5 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 6: Retrieve node(s) tag(s). User is Authenticated but not authorized for SOME nodes.
             *      Nodes:                     [ xxxx0, xxxx3, xxx4]
             *                                      * User owns xxxx3, xxxx4,  but not xxxx0
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase6 = new List<string> { };
            var nodeListCase6 = new List<int> { 0, 3, 4 };
            var resultCase6 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 7: Retrieve node(s) tag(s). User is Authenticated but not authorized for node.
             *      Nodes:                     [ xxxx0]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase7 = new List<string> { };
            var nodeListCase7 = new List<int> { 0 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 8: Retrieve node(s) tag(s). User is not authenticated.
             *      Nodes:                     [ xxxx0]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "guest", "guest", "");
            var tagListCase8 = new List<string> { };
            var nodeListCase8 = new List<int> { 0 };
            var resultCase8 = IMessageBank.Responses.notAuthenticated;

            /**
             *  Case 9: Retrieve node(s) tag(s). User is not enabled.
             *      Nodes:                     [ xxxx0]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity9 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            var tagListCase9 = new List<string> { };
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notEnabled;

            /**
             *  Case 10: Retrieve node(s) tag(s). User is not confirmed.
             *      Nodes:                     [ xxxx6]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity10 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            var tagListCase10 = new List<string> { };
            var nodeListCase10 = new List<int> { 6 };
            var resultCase10 = IMessageBank.Responses.notConfirmed;

            /**
             *  Case 11: Retrieve node(s) tag(s). Account does not exist.
             *      Nodes:                     [ xxxx0]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "user");
            var tagListCase11 = new List<string> { };
            var nodeListCase11 = new List<int> { 0 };
            var resultCase11 = IMessageBank.Responses.accountNotFound;

            /**
             *  Case 12: Retrieve node(s) tag(s). User is Authenticated. Node list is null.
             *      Nodes:                     [ xxxx0]
             *      Result Tag List:           [ ]
             */
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase12 = new List<string> { };
            List<int> nodeListCase12 = null;
            var resultCase12 = IMessageBank.Responses.nodeNotFound;

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
                new object[] { roleIdentity11, nodeListCase11, tagListCase11, resultCase11 },
                new object[] { roleIdentity12, nodeListCase12, tagListCase12, resultCase12 }
            };
        }

        /// <summary>
        ///     Tag data to remove tag from node(s).
        ///     <br>Case 0: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and already contain tag.</br>
        ///     <br>Case 1: Remove nodes tags. User is Authenticated and Authorized. Nodes exist and do not contain tag.</br>
        ///     <br>Case 2: Remove nodes tags. User is Authenticated and Authorized. Node exists and contains tag.</br>
        ///     <br>Case 3: Remove nodes tags. User is Authenticated and Authorized. Node exists and does not contain tag.</br>
        ///     <br>Case 4: Remove nodes tags. User is Authenticated. No node passed in.</br>
        ///     <br>Case 5: Remove nodes tags. User is Authenticated and Authorized. Node exists but tag doesn't exist.</br>
        ///     <br>Case 6: Remove nodes tags. User is Authenticated and Authorized. Some nodes already contain tag.</br>
        ///     <br>Case 7: Remove nodes tags. User is authenticated but not authorized to make changes to ALL nodes.</br>
        ///     <br>Case 8: Remove nodes tags. User is authenticatd but not authorized to make changes to SOME nodes.</br>
        ///     <br>Case 9: Remove nodes tags. User is authenticatd but not authorized to make changes to node.</br>
        ///     <br>Case 10: Remove nodes tags. User is not authenticated.</br>
        ///     <br>Case 11: Remove nodes tags. User is not enabled.</br>
        ///     <br>Case 12: Remove nodes tags. Account does not exist.</br>
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Test data to retrieve tags from tag bank.
        ///     <br>Case 0: Retrieve tags from tag bank. User is Authenticated and Authorized. Authorization level: user</br>
        ///     <br>Case 1: Retrieve tags from tag bank. User is Authenticated and Authorized. Authorization level: admin</br>
        ///     <br>Case 2: Retrieve tags from tag bank. User is not authenticated</br>
        ///     <br>Case 3: Retrieve tags from tag bank. User is not enabled</br>
        ///     <br>Case 4: Retrieve tags from tag bank. User is not confirmed</br>
        ///     <br>Case 5: Retrieve tags from tag bank. User has unknown role</br>
        ///     <br>Case 6: Retrieve tags from tag bank. Account does not exist</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetTagData()
        {
            /**
             *  Case 0: Retrieve tags from tag bank. User is Authenticated and Authorized. Authorization level: user
             *      Account:                    tagControllerIntegration1@tresearch.system
             *      Role:                       user
             */
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegration1@tresearch.system", "user");
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 1: Retrieve tags from tag bank. User is Authenticated and Authorized. Authorization level: admin
             *      Account:                    tagControllerIntegrationAdmin1@tresearch.system
             *      Role:                       admin
             */
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 2: Retrieve tags from tag bank. User is not authenticated
             *      Account:                    guest
             *      Role:                       guest
             */
            var roleIdentity2 = new RoleIdentity(false, "guest", "guest");
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 3: Retrieve tags from tag bank. User is not enabled
             *      Account:                    tagControllerIntegrationNotEnabled@tresearch.system
             *      Role:                       user
             */
            var roleIdentity3 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 4: Retrieve tags from tag bank. User is not confirmed
             *      Account:                    tagControllerIntegrationNotConfirmed@tresearch.system
             *      Role:                       user
             */
            var roleIdentity4 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            var resultCase4 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 5: Retrieve tags from tag bank. User has unknown role
             *      Account:                    tagControllerIntegrationAdmin1@tresearch.system
             *      Role:                       wrong
             */
            var roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "wrong");
            var resultCase5 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 6: Retrieve tags from tag bank. Account does not exist
             *      Account:                    tagControllerNoAccount@tresearch.system
             *      Role:                       user
             */
            var roleIdentity6 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "user");
            var resultCase6 = IMessageBank.Responses.tagGetSuccess;

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
