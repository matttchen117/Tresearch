using Microsoft.Extensions.DependencyInjection;
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
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(string tag, string statusCode, List<long> nodeIDs)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            string result = await recoveryService.AddTagToNodesAsync(nodeIDs, tag).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        public static IEnumerable<object[]> AddNodeTagData()
        {
            var testCase1 = new List<long> { 1, 2, 3 };     // 3 tags, all three do not have the tag
            var testCase2 = new List<long> { 1 };           // Only one tag in list, does not have tag
            var testCase3 = new List<long> { };             // No node passed in
            return new[]
            {
                new object[] { "art", "200: Server: success", testCase1 },
                new object[] { "cooking", "200: Server: success", testCase2 },
                new object[] { "baking", "204: No nodes passed in", testCase3 }
            };
        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveTagFromNodeAsync(string tag, string statusCode, List<long> nodeIDs)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();

            //Act
            string result = await recoveryService.RemoveTagFromNodesAsync(nodeIDs, tag).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            var testCase1 = new List<long> { 1, 2, 3 };     // 3 tags, all three do not have the tag
            var testCase2 = new List<long> { 1 };           // Only one tag in list, does not have tag
            var testCase3 = new List<long> { };             // No node passed in
            return new[]
            {
                new object[] { "archery", "200: Server: success", testCase1 },
                new object[] { "calculus", "200: Server: success", testCase2 },
                new object[] { "baking", "204: No nodes passed in", testCase3 }
            };
        }

        [Theory]
        [MemberData(nameof(GetTagFromNodesData))]
        public async Task GetTagsFromNodesAsync(string statusCode, List<long> nodeIDs, List<string> tags)
        {
            //arrange
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
            var testCase1 = new List<long> { 1, 2, 3 };     // 3 tags, all three do not have the tag
            var testCase2 = new List<long> { 1 };           // Only one tag in list, does not have tag
            var testCase3 = new List<long> { };             // No node passed in

            var tagCase1 = new List<string> { "archery" };
            var tagCase2 = new List<string> { "archery", "calculus" };
            var tagCase3 = new List<string> { };
            return new[]
            {
                new object[] { "200: Server: success", testCase1, tagCase1 },
                new object[] { "200: Server: success", testCase2, tagCase2 },
                new object[] { "204: No nodes passed in", testCase3, tagCase3 }
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
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            string statusCode = "200: Server: success";

            //Act
            Tuple<List<string>, string> results = await recoveryService.GetTagsAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, results.Item2);
        }
    }
}
