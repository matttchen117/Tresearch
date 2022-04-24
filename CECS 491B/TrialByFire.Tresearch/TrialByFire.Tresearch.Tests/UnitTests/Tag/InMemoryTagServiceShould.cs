using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Tag
{
    public class InMemoryTagServiceShould : TestBaseClass    
    {
        public InMemoryTagServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ITagService, TagService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ITagService tagService = TestProvider.GetService<ITagService>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagService.AddTagToNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTagAsync(string tagName, int count, IMessageBank.Responses response)
        {
            //Arrange
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ITagService tagService = TestProvider.GetService<ITagService>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagService.CreateTagAsync(tagName, count, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ITagService tagService = TestProvider.GetService<ITagService>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagService.RemoveTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetTagsAsync()
        {
            //Arrange
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ITagService tagService = TestProvider.GetService<ITagService>();
            string expected = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> results = await tagService.GetTagsAsync(cancellationTokenSource.Token);
            string result = results.Item2;

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

        public static IEnumerable<object[]> CreateTagData()
        {
            //Tag does not exist
            var tagName0  = "Tresearch Service Create tag1";
            var count0    = 0;
            var expected0 = IMessageBank.Responses.tagCreateSuccess;


            //Tag already exists
            var tagName1  = "Tresearch Service Create tag2";
            var count1    = 0;
            var expected1 = IMessageBank.Responses.tagDuplicate;

            return new[]
           {
                new object[] { tagName0, count0, expected0 },
                new object[] { tagName1, count1, expected1 }
            };

        }


        public static IEnumerable<object[]> DeleteTagData()
        {
            //Tag exists
            var tagName0 = "Tresearch Service Delete tag1";
            var expected0 = IMessageBank.Responses.tagDeleteSuccess;


            //Tag does not exists
            var tagName1 = "Tresearch Service Delete tag2";
            var expected1 = IMessageBank.Responses.tagDeleteSuccess;

            //Tag exists but other nodes have tagged
            var tagName2 = "Tresearch Service Delete tag3";
            var expected2 = IMessageBank.Responses.tagDeleteSuccess;

            return new[]
           {
                new object[] { tagName0, expected0 },
                new object[] { tagName1, expected1 },
                new object[] { tagName2, expected2 }
            };
        }
    }
}
