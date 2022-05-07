using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

namespace TrialByFire.Tresearch.Tests.UnitTests.Tag
{
    public class InMemoryTagControllerShould : TestBaseClass
    {
        public InMemoryTagControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<ITagManager, TagManager>();
            TestServices.AddScoped<ITagController, TagController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            // Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultAdd = await tagController.AddTagToNodesAsync(nodeIDs, tagName);
            var result = resultAdd as ObjectResult;

            // Assert
            Assert.NotNull(resultAdd);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultCreate = await tagController.CreateTagAsync(tagName);
            var result = resultCreate as ObjectResult;

            // Assert
            Assert.NotNull(resultCreate);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);

        }

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            // Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultDelete = await tagController.DeleteTagAsync(tagName);
            var result = resultDelete as ObjectResult;

            // Assert
            Assert.NotNull(resultDelete);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);
        }

        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, List<string> expectedTags, IMessageBank.Responses response)
        {
            // Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultGet = await tagController.GetNodeTagsAsync(nodeIDs);
            var result = resultGet as ObjectResult;

            // Assert
            Assert.NotNull(resultGet);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedTags, result.Value);
        }

        [Theory]
        [MemberData(nameof(GetTagData))]
        public async Task GetTagsAsync(IRoleIdentity roleIdentity, IMessageBank.Responses response)
        {
            // Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultGet = await tagController.GetTagsAsync();
            var result = resultGet as ObjectResult;

            // Assert
            Assert.NotNull(resultGet);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);

        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveNodeTagsAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            // Arrange
            ITagController tagController = TestProvider.GetService<ITagController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            string expected = await messageBank.GetMessage(response);
            string[] exps = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(exps[2])
            { StatusCode = Convert.ToInt32(exps[0]) };

            // Act
            IActionResult resultRemove = await tagController.RemoveTagFromNodesAsync(nodeIDs, tagName);
            var result = resultRemove as ObjectResult;

            // Assert
            Assert.NotNull(resultRemove);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
            Assert.Equal(expectedResult.Value, result.Value);

        }

        public static IEnumerable<object[]> AddNodeTagData()
        {
            //User Authorized, Nodes already have tag
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase0 = "Tresearch Controller Add Tag1";
            var nodeListCase0 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;

            //User Authorized, Nodes do not contain these tags
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase1 = "Tresearch Controller Add Tag2";
            var nodeListCase1 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            //User Authorized, Node already has tag
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase2 = "Tresearch Controller Add Tag3";
            var nodeListCase2 = new List<long> { 9019303350 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase3 = "Tresearch Controller Add Tag4";
            var nodeListCase3 = new List<long> { 9019303350 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase4 = "Tresearch Controller Add Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            //User Authorized, Tag doesn't exist
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase5 = "Tresearch Controller Add Tag5";
            var nodeListCase5 = new List<long> { 9019303350 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            //User Authorized, Some Nodes already have tag (9019303351 contains tag)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase6 = "Tresearch Controller Add Tag3";
            var nodeListCase6 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase6 = IMessageBank.Responses.tagAddSuccess;

            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase7 = "Tresearch Controller Add Tag3";
            var nodeListCase7 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 9019303353, 9019303354 but not 9019303350)
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase8 = "Tresearch Controller Add Tag3";
            var nodeListCase8 = new List<long> { 9019303350, 9019303353, 9019303354 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase9 = "Tresearch Controller Add Tag3";
            var nodeListCase9 = new List<long> { 9019303350 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Controller Add Tag3";
            var nodeListCase10 = new List<long> { 9019303350 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagControllerIntegrationNotEnabled@tresearch.system", "user", "3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee");
            var tagNameCase11 = "Tresearch Manager Add Tag3";
            var nodeListCase11 = new List<long> { 9019303365 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagControllerIntegrationNotConfirmed@tresearch.system", "user", "89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c");
            var tagNameCase12 = "Tresearch Controller Add Tag3";
            var nodeListCase12 = new List<long> { 9019303366 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagControllerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Controller Add Tag3";
            var nodeListCase13 = new List<long> { 0 };
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

        public static IEnumerable<object[]> CreateTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Controller Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagCreateSuccess;

            //User is Authenticated and Authorized as admin, tag exists
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Controller Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDuplicate;

            //User is Authenticated but not authorized (user)
            var roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Controller Doesnt Matter1";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            //User is not Authenticated
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Controller Doesnt Matter2";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;


            //UserAccount  does not exist
            var roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Controller Doesnt Matter3";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 }
            };
        }

        public static IEnumerable<object[]> DeleteTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Controller REMOVE Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagDeleteSuccess;

            //User is Authenticated and Authorized as admin, tag exists
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Controller REMOVE Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDeleteSuccess;

            //User is Authenticated but not authorized (user)
            var roleIdentity2 = new RoleIdentity(false, "tagControllerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Controller Doesnt Matter";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            //User is not Authenticated
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Controller Doesnt Matter";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;


            //UserAccount  does not exist
            var roleIdentity4 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Controller Doesnt Matter";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

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

        public static IEnumerable<object[]> GetNodeTagData()
        {
            /*User Authorized, Nodes contain shared tags but also have other tags
             *          9019303356: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *          9019303357: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2
             *          9019303358: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             */

            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase0 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase0 = new List<long> { 9019303356, 9019303357, 9019303358 };
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /*User Authorized, Nodes do not contain shared tags
             *          9019303358: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2, Tresearch Controller Get Tag3
             *          9019303359: Tresearch Controller Get Tag2
             *          9019303360: Tresearch Controller Get Tag1
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase1 = new List<string> { };
            var nodeListCase1 = new List<long> { 9019303358, 9019303359, 9019303360 };
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Grab Single Node's Tags (9019303356: Tresearch Controller Get Tag1, Tresearch Controller Get Tag2)
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase2 = new List<string> { "Tresearch Controller Get Tag1", "Tresearch Controller Get Tag2" };
            var nodeListCase2 = new List<long> { 9019303356 };
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase3 = new List<string> { };
            var nodeListCase3 = new List<long> { 9019303361 };
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration3@tresearch.system", "user", "0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f");
            var tagListCase4 = new List<string> { };
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;


            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase5 = new List<string> { };
            var nodeListCase5 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase5 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 9019303353, 9019303354 but not 9019303350)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase6 = new List<string> { };
            var nodeListCase6 = new List<long> { 9019303350, 9019303353, 9019303354 };
            var resultCase6 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagListCase7 = new List<string> { };
            var nodeListCase7 = new List<long> { 9019303350 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "guest", "guest", "");
            var tagListCase8 = new List<string> { };
            var nodeListCase8 = new List<long> { 9019303350 };
            var resultCase8 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity9 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            var tagListCase9 = new List<string> { };
            var nodeListCase9 = new List<long> { 9019303365 };
            var resultCase9 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity10 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            var tagListCase10 = new List<string> { };
            var nodeListCase10 = new List<long> { 9019303366 };
            var resultCase10 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagControllerNoAccount@tresearch.system", "user");
            var tagListCase11 = new List<string> { };
            var nodeListCase11 = new List<long> { 0 };
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

        public static IEnumerable<object[]> GetTagData()
        {
            //User is Authenticated and Authorized 
            var roleIdentity0 = new RoleIdentity(false, "tagControllerIntegration1@tresearch.system", "user");
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            //User is Authenticated and Authorized and Admin
            var roleIdentity1 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "admin");
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User has not been authenticated
            var roleIdentity2 = new RoleIdentity(false, "guest", "guest");
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User is not enabled
            var roleIdentity3 = new RoleIdentity(false, "tagControllerIntegrationNotEnabled@tresearch.system", "user");
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User is not confirmed
            var roleIdentity4 = new RoleIdentity(false, "tagControllerIntegrationNotConfirmed@tresearch.system", "user");
            var resultCase4 = IMessageBank.Responses.tagGetSuccess;

            //User is has unknown role
            var roleIdentity5 = new RoleIdentity(false, "tagControllerIntegrationAdmin1@tresearch.system", "wrong");
            var resultCase5 = IMessageBank.Responses.tagGetSuccess;

            //UserAccount not found
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

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            //User Authorized, Nodes already have tag
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase0 = "Tresearch Controller Delete Tag1";
            var nodeListCase0 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase0 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Nodes do not contain these tags
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase1 = "Tresearch Controller Delete Tag2";
            var nodeListCase1 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase1 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node already has tag
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase2 = "Tresearch Controller Delete Tag3";
            var nodeListCase2 = new List<long> { 9019303350 };
            var resultCase2 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase3 = "Tresearch Controller Delete Tag4";
            var nodeListCase3 = new List<long> { 9019303350 };
            var resultCase3 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase4 = "Tresearch Controller Delete Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            //User Authorized, Tag doesn't exist
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase5 = "Tresearch Controller Delete Tag5";
            var nodeListCase5 = new List<long> { 9019303350 };
            var resultCase5 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Some Nodes already have tag (9019303351 contains tag)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagControllerIntegration1@tresearch.system", "user", "09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5");
            var tagNameCase6 = "Tresearch Controller Delete Tag3";
            var nodeListCase6 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase6 = IMessageBank.Responses.tagRemoveSuccess;

            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase7 = "Tresearch Controller Delete Tag3";
            var nodeListCase7 = new List<long> { 9019303350, 9019303351, 9019303352 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 9019303353, 9019303354 but not 9019303350)
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase8 = "Tresearch Controller Delete Tag3";
            var nodeListCase8 = new List<long> { 9019303350, 9019303353, 9019303354 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagControllerIntegration2@tresearch.system", "user", "20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb");
            var tagNameCase9 = "Tresearch Controller Delete Tag3";
            var nodeListCase9 = new List<long> { 9019303350 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Controller Delete Tag3";
            var nodeListCase10 = new List<long> { 9019303350 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagControllerIntegrationNotEnabled@tresearch.system", "user", "3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee");
            var tagNameCase11 = "Tresearch Controller Delete Tag3";
            var nodeListCase11 = new List<long> { 9019303365 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagControllerIntegrationNotConfirmed@tresearch.system", "user", "89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c");
            var tagNameCase12 = "Tresearch Controller Delete Tag3";
            var nodeListCase12 = new List<long> { 9019303366 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagControllerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Controller Delete Tag3";
            var nodeListCase13 = new List<long> { 0 };
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
    }    
}
