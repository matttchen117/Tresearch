using Microsoft.Extensions.DependencyInjection;

using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    public class TagServiceShould : TestBaseClass
    {
        public TagServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<IMessageBank, MessageBank>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(string currentIdentity, string currentRole, string tag, string statusCode, List<long> nodeIDs)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.AddTagToNodesAsync(nodeIDs, tag, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }
        
        public static IEnumerable<object[]> AddNodeTagData()
        {
            //User owns all tags, nodes already have tag
            var userCase0 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase0 = "user";
            var tagNameCase0 = "music";
            var nodeListCase0 = new List<long> { 67890, 67891, 67892 };
            var resultCase0 = "200: Server: success";

            //User owns all tags, nodes do not contain these tags already
            var userCase1 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase1 = "user";
            var tagNameCase1 = "art";
            var nodeListCase1 = new List<long> { 67890, 67891, 67892 };
            var resultCase1 = "200: Server: success";

            // User owns tag, node doesn't already contain tag
            var userCase2 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase2 = "user";
            var tagNameCase2 = "cooking";
            var nodeListCase2 = new List<long> { 67890};
            var resultCase2 = "200: Server: success";

            //No node is passed in
            var userCase3 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase3 = "user";
            var tagNameCase3 = "baking";
            var nodeListCase3 = new List<long> { };
            var resultCase3 = "204: No nodes passed in.";

            //User does not own node and is trying to make changes
            var userCase4 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase4 = "user";
            var tagNameCase4 = "art";
            var nodeListCase4 = new List<long> { 67890 };
            var resultCase4 = "403: Database: You are not authorized to perform this operation.";

            //User does not own all nodes and is trying to make changes (they only own first 2)
            var userCase5 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase5 = "user";
            var tagNameCase5 = "baking";
            var nodeListCase5 = new List<long> { 67890, 67891, 67897 };     //user does not own 67897
            var resultCase5 = "403: Database: You are not authorized to perform this operation.";

            //Users role is not valid
            var userCase6 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase6 = "role";         // This role is invalid
            var tagNameCase6 = "baking";
            var nodeListCase6 = new List<long> { 67890 };     
            var resultCase6 = "400: Server: Unknown role used.";

            return new[]
            {
                new object[] { userCase0, roleCase0, tagNameCase0, resultCase0, nodeListCase0 },
                new object[] { userCase1, roleCase1, tagNameCase1, resultCase1, nodeListCase1 },
                new object[] { userCase2, roleCase2, tagNameCase2, resultCase2, nodeListCase2 },
                new object[] { userCase3, roleCase3, tagNameCase3, resultCase3, nodeListCase3 },
                new object[] { userCase4, roleCase4, tagNameCase4, resultCase4, nodeListCase4 },
                new object[] { userCase5, roleCase5, tagNameCase5, resultCase5, nodeListCase5 },
                new object[] { userCase6, roleCase6, tagNameCase6, resultCase6, nodeListCase6 }
            };
        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveTagFromNodeAsync(string currentIdentity, string currentRole, string tag, string statusCode, List<long> nodeIDs)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.RemoveTagFromNodesAsync(nodeIDs, tag, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            // Valid User owns all nodes, all nodes contain tag
            var userCase0 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase0 = "user";
            var tagNameCase0 = "gym";
            var nodeListCase0 = new List<long> { 67890, 67891, 67892 };
            var resultCase0 = "200: Server: success";

            // Valid User owns node, node contains tag
            var userCase1 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase1 = "user";
            var tagNameCase1 = "myth";
            var nodeListCase1 = new List<long> { 67890 };
            var resultCase1 = "200: Server: success";

            // Valid user owns all nodes, nodes do not contain tag
            var userCase2 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase2 = "user";
            var tagNameCase2 = "algebra";
            var nodeListCase2 = new List<long> { 67890, 67891, 67892 };
            var resultCase2 = "200: Server: success";

            // Valid user owns node, node does not contain tag
            var userCase3 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase3 = "user";
            var tagNameCase3 = "algebra";
            var nodeListCase3 = new List<long> { 67890 };
            var resultCase3 = "200: Server: success";

            // Valid User but passsed in no nodes
            var userCase4 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase4 = "user";
            var tagNameCase4 = "baking";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = "204: No nodes passed in.";

            // Valid user does not own nodes
            var userCase5 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase5 = "user";
            var tagNameCase5 = "art";
            var nodeListCase5 = new List<long> { 67890, 67891, 67892 };
            var resultCase5 = "403: Database: You are not authorized to perform this operation.";

            // Valid user does not own all tags (only first 2)
            var userCase6 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase6 = "user";
            var tagNameCase6 = "baking";
            var nodeListCase6 = new List<long> { 67890, 67891, 67897 };     //user does not own 67897
            var resultCase6 = "403: Database: You are not authorized to perform this operation.";

            // Valid user does not own node
            var userCase7 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase7 = "user";
            var tagNameCase7 = "art";
            var nodeListCase7 = new List<long> { 67890 };
            var resultCase7 = "403: Database: You are not authorized to perform this operation.";

            // Invalid user
            var userCase8 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase8 = "role";
            var tagNameCase8 = "baking";
            var nodeListCase8 = new List<long> { 67890 };     
            var resultCase8 = "400: Server: Unknown role used.";

            return new[]
            {
                new object[] {userCase0, roleCase0, tagNameCase0, resultCase0, nodeListCase0},
                new object[] {userCase1, roleCase1, tagNameCase1, resultCase1, nodeListCase1},
                new object[] {userCase2, roleCase2, tagNameCase2, resultCase2, nodeListCase2},
                new object[] {userCase3, roleCase3, tagNameCase3, resultCase3, nodeListCase3},
                new object[] {userCase4, roleCase4, tagNameCase4, resultCase4, nodeListCase4},
                new object[] {userCase5, roleCase5, tagNameCase5, resultCase5, nodeListCase5},
                new object[] {userCase6, roleCase6, tagNameCase6, resultCase6, nodeListCase6},
                new object[] {userCase7, roleCase7, tagNameCase7, resultCase7, nodeListCase7},
                new object[] {userCase8, roleCase8, tagNameCase8, resultCase8, nodeListCase8}
            };
        }

        [Theory]
        [MemberData(nameof(GetTagFromNodesData))]
        public async Task GetTagsFromNodesAsync(string currentIdentity, string currentRole, string statusCode, List<long> nodeIDs, List<string> tags)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            Tuple<List<string>, string> results = await recoveryService.GetNodeTagsAsync(nodeIDs).ConfigureAwait(false);
            string result = results.Item2;
            List<string> resultTags = results.Item1;

            //Assert
            Assert.Equal(statusCode, result);
            Assert.Equal(tags, resultTags);
        }

        public static IEnumerable<object[]> GetTagFromNodesData()
        {
            // Valid user but doesn't own nodes
            var userCase0 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase0 = "user";
            var nodeListCase0 = new List<long> { 67890, 67891, 67892 };
            var tagListCase0 = new List<string> { };
            var resultCase0 = "403: Database: You are not authorized to perform this operation.";

            // Valid user but doesn't own node
            var userCase1 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase1 = "user";
            var nodeListCase1 = new List<long> { 67890 };
            var tagListCase1 = new List<string> { };
            var resultCase1 = "403: Database: You are not authorized to perform this operation.";

            // Valid user owns nodes
            var userCase2 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase2 = "user";
            var nodeListCase2 = new List<long> { 67893, 67894, 67895 };
            var tagListCase2 = new List<string> { "craft"};
            var resultCase2 = "200: Server: success";

            // Valid user owns nodes 
            var userCase3 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase3 = "user";
            var nodeListCase3 = new List<long> { 67893, 67894 };
            var tagListCase3 = new List<string> { "craft", "crypto" };
            var resultCase3 = "200: Server: success";

            // Valid user owns node
            var userCase4 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase4 = "user";
            var nodeListCase4 = new List<long> { 67893 };
            var tagListCase4 = new List<string> { "craft", "crypto", "fries"};
            var resultCase4 = "200: Server: success";

            // Valid user but passed in empty list of nodes
            var userCase5 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase5 = "user";
            var nodeListCase5 = new List<long> {  };
            var tagListCase5 = new List<string> {  };
            var resultCase5 = "204: No nodes passed in.";

            // Invalid user
            var userCase6 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase6 = "role";
            var nodeListCase6 = new List<long> { };
            var tagListCase6 = new List<string> { };
            var resultCase6 = "400: Server: Unknown role used.";
            
            return new[]
            {
                new object[] {userCase0, roleCase0, resultCase0, nodeListCase0, tagListCase0},
                new object[] {userCase1, roleCase1, resultCase1, nodeListCase1, tagListCase1},
                new object[] {userCase2, roleCase2, resultCase2, nodeListCase2, tagListCase2},
                new object[] {userCase3, roleCase3, resultCase3, nodeListCase3, tagListCase3},
                new object[] {userCase4, roleCase4, resultCase4, nodeListCase4, tagListCase4},
                new object[] {userCase5, roleCase5, resultCase5, nodeListCase5, tagListCase5},
                new object[] {userCase6, roleCase6, resultCase6, nodeListCase6, tagListCase6}
            };
        }

        [Theory]
        [InlineData("CECS 491B", "200: Server: success")]
        [InlineData("Trial By Fire", "409: Database: The tag already exists.")]

        public async Task CreateTagAsync(string tagName, string statusCode)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            string result = await recoveryService.CreateTagAsync(tagName).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        [Theory]
        [InlineData("CSULB", "200: Server: success")]                      // Tag exists
        [InlineData("Long Beach", "200: Server: success")]                 // Tag doesn't exist
        [InlineData("Oh no Im tagged", "200: Server: success")]           // Tag exists and is currently tagged on nodes
        public async Task RemoveTagAsync(string tagName, string statusCode)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            string result = await recoveryService.RemoveTagAsync(tagName).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        [Fact]
        public async Task GetTagsAsync()
        {
            //Arrange
            ITagService tagService = TestProvider.GetService<ITagService>();
            string statusCode = "200: Server: success";

            //Act
            Tuple<List<string>, string> results = await tagService.GetTagsAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, results.Item2);
        }

        [Theory]
        [MemberData(nameof(SetUpAuthorizationTest))]
        public async Task CheckIfAuthorized(List<long> nodeID, IAccount account, bool expected, string expectedStatusCode)
        {
            //Arrange
            ITagService tagService = TestProvider.GetService<ITagService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<bool, string> results = await tagService.OwnsNode(nodeID, account, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, results.Item1);
            Assert.Equal(expectedStatusCode, results.Item2);
        }

        public static IEnumerable<object[]> SetUpAuthorizationTest()
        {
            var nodesCase1 = new List<long> { 67893, 67894, 67895 }; ;
            var AccountCase1 = new Account("82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b", "user");
            var expectedBoolCase1 = true;
            var expectedStatusCase1 = "200: Server: success";

            return new[]
            {
                new object[] { nodesCase1, AccountCase1, expectedBoolCase1, expectedStatusCase1}
            };
        }
    }
}
