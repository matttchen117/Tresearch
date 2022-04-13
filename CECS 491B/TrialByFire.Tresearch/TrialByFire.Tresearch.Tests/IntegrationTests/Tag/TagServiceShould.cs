using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    public class TagServiceShould : TestBaseClass, IClassFixture<TagServiceDatabaseFixture>
    {
        public TagServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<IMessageBank, MessageBank>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(List<long> nodeIDs, string tag, IMessageBank.Responses response)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.AddTagToNodesAsync(nodeIDs, tag, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> AddNodeTagData()
        {
            //Nodes already have tag
            var tagNameCase0 = "Tresearch Service Add Tag1";
            var nodeListCase0 = new List<long> { 2072942630, 2072942631, 2072942632 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;

            //Nodes do not contain these tags
            var tagNameCase1 = "Tresearch Service Add Tag2";
            var nodeListCase1 = new List<long> { 2072942630, 2072942631, 2072942632 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            //Node already has tag
            var tagNameCase2 = "Tresearch Service Add Tag3";
            var nodeListCase2 = new List<long> { 2072942630 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            //Node does not have tag
            var tagNameCase3 = "Tresearch Service Add Tag4";
            var nodeListCase3 = new List<long> { 2072942630 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            //No node is passed in
            var tagNameCase4 = "Tresearch Service Add Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            //Tag doesn't exist
            var tagNameCase5 = "Tresearch Service Add Tag5";
            var nodeListCase5 = new List<long> { 2072942630 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            //Some Nodes already have tag (2072942630 contains tag)
            var tagNameCase6 = "Tresearch Service Add Tag3";
            var nodeListCase6 = new List<long> { 2072942630, 2072942631, 2072942632 };
            var resultCase6 = IMessageBank.Responses.tagAddSuccess;

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { nodeListCase5, tagNameCase5, resultCase5 },
                new object[] { nodeListCase6, tagNameCase6, resultCase6 }
            };
        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveTagFromNodeAsync(List<long> nodeIDs, string tag, string statusCode)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.RemoveTagFromNodesAsync(nodeIDs, tag, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            //Nodes already have tag
            var tagNameCase0 = "Tresearch Service Delete Tag1";
            var nodeListCase0 = new List<long> { 2072942633, 2072942634, 2072942635 };
            var resultCase0 = "200: Server: Tag removed from node(s).";

            //Nodes do not contain these tags
            var tagNameCase1 = "Tresearch Service Delete Tag2";
            var nodeListCase1 = new List<long> { 2072942633, 2072942634, 20729426352 };
            var resultCase1 = "200: Server: Tag removed from node(s).";

            //Node already has tag
            var tagNameCase2 = "Tresearch Service Delete Tag3";
            var nodeListCase2 = new List<long> { 2072942633 };
            var resultCase2 = "200: Server: Tag removed from node(s).";

            //Node does not have tag
            var tagNameCase3 = "Tresearch Service Delete Tag4";
            var nodeListCase3 = new List<long> { 2072942633 };
            var resultCase3 = "200: Server: Tag removed from node(s).";

            //No node is passed in
            var tagNameCase4 = "Tresearch Service Delete Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = "404: Database: The node was not found.";

            //Tag doesn't exist
            var tagNameCase5 = "Tresearch Service Delete Tag5";
            var nodeListCase5 = new List<long> { 2072942633 };
            var resultCase5 = "200: Server: Tag removed from node(s).";

            //Some Nodes  have tag (2072942630 contains tag)
            var tagNameCase6 = "Tresearch Service Delete Tag3";
            var nodeListCase6 = new List<long> { 2072942633, 2072942634, 20729426352 };
            var resultCase6 = "200: Server: Tag removed from node(s).";

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { nodeListCase5, tagNameCase5, resultCase5 },
                new object[] { nodeListCase6, tagNameCase6, resultCase6 }
            };
        }

        [Theory]
        [MemberData(nameof(GetTagFromNodesData))]
        public async Task GetTagsFromNodesAsync( List<long> nodeIDs, string statusCode, List<string> tags)
        {
            //Arrange
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
            /**Nodes contain shared tags
             *      2072942636: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2', 'Tresearch Service Get Tag3'
             *      2072942637: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2'
             *      2072942638: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2'
             */
            var nodeListCase0 = new List<long> { 2072942636, 2072942637, 2072942638 };
            string expectedCase0 = "200: Server: Tag(s) retrieved.";
            var expectedTags0 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2" };

            /**Nodes contain no shared tags
             *      2072942639: 'Tresearch Service Get Tag1'
             *      2072942640: 'Tresearch Service Get Tag2'
             *      2072942641: 'Tresearch Service Get Tag3'
             */
            var nodeListCase1 = new List<long> { 2072942639, 2072942640, 2072942641 };
            string expectedCase1 = "200: Server: Tag(s) retrieved.";
            var expectedTags1 = new List<string> { };

            //Node has tags: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2', 'Tresearch Service Get Tag3'
            var nodeListCase2 = new List<long> { 2072942636 };
            string expectedCase2 = "200: Server: Tag(s) retrieved.";
            var expectedTags2 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2", "Tresearch Service Get Tag3" };

            //Node contains no tags: 
            var nodeListCase3 = new List<long> { 2072942642 };
            string expectedCase3 = "200: Server: Tag(s) retrieved.";
            var expectedTags3 = new List<string> { };

            //No nodes passed in
            var nodeListCase4 = new List<long> { };
            string expectedCase4 = "404: Database: The node was not found.";
            var expectedTags4 = new List<string> { };

            return new[]
            {
                new object[] { nodeListCase0, expectedCase0, expectedTags0 },
                new object[] { nodeListCase1, expectedCase1, expectedTags1},
                new object[] { nodeListCase2, expectedCase2, expectedTags2 },
                new object[] { nodeListCase3, expectedCase3, expectedTags3 },
                new object[] { nodeListCase4, expectedCase4, expectedTags4 }
            };
        }

        [Theory]
        [InlineData("Tresearch Service Create tag1", 0, "200: Server: Tag created in tag bank.")]
        [InlineData("Tresearch Service Create tag2", 4, "409: Database: The tag already exists.")]

        public async Task CreateTagAsync(string tagName, int count, string statusCode)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            string result = await recoveryService.CreateTagAsync(tagName, count).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        [Theory]
        //Tag Doesn't exist
        [InlineData("Tresearch Service Remove Me tag1", "200: Server: Tag removed from tag bank.")]
        //Tag Exists
        [InlineData("Tresearch Service Remove Me tag2", "200: Server: Tag removed from tag bank.")]
        // Tag exists and is currently tagged on nodes
        [InlineData("Tresearch Service Remove Me tag3", "200: Server: Tag removed from tag bank.")]           
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
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            //Act
            Tuple<List<ITag>, string> results = await tagService.GetTagsAsync();
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
        }
    }

    public class TagServiceDatabaseFixture : TestBaseClass, IDisposable
    {
        public TagServiceDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ServiceIntegrationSetup.sql");

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

        public void Dispose()
        {
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ServiceIntegrationCleanup.sql");

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
