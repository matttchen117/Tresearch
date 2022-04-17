using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Tag
{
    public class InMemoryTagManagerShould: TestBaseClass
    {
        public InMemoryTagManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<ITagManager, TagManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.AddTagToNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task RemoveTagFromNodeAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.RemoveTagFromNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }


        public async Task GetNodeTagsAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, List<string> tagsExpected, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<string>, string> results = await tagManager.GetNodeTagsAsync(nodeIDs, cancellationTokenSource.Token);
            List<string> tagsResults = results.Item1;
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
            Assert.Equal(tagsExpected, tagsResults);
        }

        public async Task CreateTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.CreateTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.CreateTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task GetTagsAsync(IRoleIdentity roleIdentity, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> results = await tagManager.GetTagsAsync(cancellationTokenSource.Token);
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AddNodeTagData()
        {
            //User Authorized, Nodes already have tag
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase0 = "Tresearch Manager Add Tag1";
            var nodeListCase0 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase0 = "200: Server: Tag added to node(s).";

            //User Authorized, Nodes do not contain these tags
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase1 = "Tresearch Manager Add Tag2";
            var nodeListCase1 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase1 = "200: Server: Tag added to node(s).";

            //User Authorized, Node already has tag
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase2 = "Tresearch Manager Add Tag3";
            var nodeListCase2 = new List<long> { 8019303350 };
            var resultCase2 = "200: Server: Tag added to node(s).";

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase3 = "Tresearch Manager Add Tag4";
            var nodeListCase3 = new List<long> { 8019303350 };
            var resultCase3 = "200: Server: Tag added to node(s).";

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase4 = "Tresearch Manager Add Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = "404: Database: The node was not found.";

            //User Authorized, Tag doesn't exist
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase5 = "Tresearch Manager Add Tag5";
            var nodeListCase5 = new List<long> { 8019303350 };
            var resultCase5 = "404: Database: Tag not found.";

            //User Authorized, Some Nodes already have tag (8019303351 contains tag)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase6 = "Tresearch Manager Add Tag3";
            var nodeListCase6 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase6 = "200: Server: Tag added to node(s).";

            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase7 = "Tresearch Manager Add Tag3";
            var nodeListCase7 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase7 = "403: Database: You are not authorized to perform this operation.";

            //User not Authorized for some Nodes (user owns 8019303353, 8019303354 but not 8019303350)
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase8 = "Tresearch Manager Add Tag3";
            var nodeListCase8 = new List<long> { 8019303350, 8019303353, 8019303354 };
            var resultCase8 = "403: Database: You are not authorized to perform this operation.";

            //User not Authorized for node
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase9 = "Tresearch Manager Add Tag3";
            var nodeListCase9 = new List<long> { 8019303350 };
            var resultCase9 = "403: Database: You are not authorized to perform this operation.";

            //User has not been authenticated
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Manager Add Tag3";
            var nodeListCase10 = new List<long> { 8019303350 };
            var resultCase10 = "401: Server: No active session found. Please login and try again.";

            //User is not enabled
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagNameCase11 = "Tresearch Manager Add Tag3";
            var nodeListCase11 = new List<long> { 8019303365 };
            var resultCase11 = "401: Database: UserAccount disabled. Perform account recovery or contact system admin.";

            //User is not confirmed
            IRoleIdentity roleIdentity12 = new RoleIdentity(false, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagNameCase12 = "Tresearch Manager Add Tag3";
            var nodeListCase12 = new List<long> { 8019303366 };
            var resultCase12 = "401: Database: Please confirm your account before attempting to login.";

            //UserAccount  does not exist
            IRoleIdentity roleIdentity13 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "user");
            var tagNameCase13 = "Tresearch Manager Add Tag3";
            var nodeListCase13 = new List<long> { };
            var resultCase13 = "500: Database: The UserAccount was not found.";

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
