using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    /// <summary>
    ///     SqlDAOShould tests the Data Access Object Layer of the Tag Feature.  
    /// </summary>
    public class SqlDAOShould : TestBaseClass, IClassFixture<TagSqlDatabaseFixture>
    {
        TagSqlDatabaseFixture fixture;                                                                   
        public SqlDAOShould(TagSqlDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddTagData))]
        public async Task AddTagAsync(List<long> nodeIDs, string tagName, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.AddTagAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        //Success: Tag Added to database
        [InlineData("Tresearch SqlDAO test tag1", 0, "200: Server: Tag created in tag bank.")]
        //Fail: Tag already exists in database
        [InlineData("Tresearch SqlDAO This Tag Exists Already", 0, "409: Database: The tag already exists.")]
        public async Task CreateTagAsync(string tagName, int count, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.CreateTagAsync(tagName, count, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }

        [Theory]
        //Success: Tag removed from database
        [InlineData("Tresearch SqlDAO Delete Me Tag", "200: Server: Tag removed from tag bank.")]
        //Success: Tag was not in database
        [InlineData("Tresearch This Tag Doesnt exist", "200: Server: Tag removed from tag bank.")]
        //Success: Tag removed from database (there were nodes that has this tagged)
        public async Task DeleteTagAsync(string tagName, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.DeleteTagAsync(tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }
        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(List<long> nodeIDs, string expected, List<string> expectedTags)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<string>, string> myResult = await sqlDAO.GetNodeTagsAsync(nodeIDs, cancellationTokenSource.Token);
            string result = myResult.Item2;
            List<string> resultTags = myResult.Item1;


            //Arrange
            Assert.Equal(expected, result);
            Assert.Equal(expectedTags, resultTags);
        }

        [Fact]
        public async Task GetTagsAsync()
        {
            //Arrange
            string expected = "200: Server: Tag(s) retrieved.";
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> resultTags = await sqlDAO.GetTagsAsync(cancellationTokenSource.Token);
            string result = resultTags.Item2;

            //Arrange
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(RemoveTagData))]
        public async Task RemoveTagAsync(List<long> nodeIDs, string tagName, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.RemoveTagAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }


        public static IEnumerable<object[]> AddTagData()
        {
            //nodes already have tag
            var tagNameCase0 = "Tresearch SqlDAO Add Tag1";
            var nodeListCase0 = new List<long> { 2022030533, 2022030534, 2022030535 };
            var resultCase0 = "200: Server: Tag added to node(s).";

            //nodes do not contain these tags already
            var tagNameCase1 = "Tresearch SqlDAO Add Tag2";
            var nodeListCase1 = new List<long> { 2022030533, 2022030534, 2022030535 };
            var resultCase1 = "200: Server: Tag added to node(s).";

            // node doesn't already contain tag
            var tagNameCase2 = "Tresearch SqlDAO Add Tag3";
            var nodeListCase2 = new List<long> { 2022030533 };
            var resultCase2 = "200: Server: Tag added to node(s).";

            //Node already has tag
            var tagNameCase3 = "Tresearch SqlDAO Add Tag4";
            var nodeListCase3 = new List<long> { };
            var resultCase3 = "200: Server: Tag added to node(s).";

            //Tag doesn't exist
            var tagNameCase4 = "Tresearch SqlDAO Add Tag5";
            var nodeListCase4 = new List<long> { 2022030533 };
            var resultCase4 = "404: Database: Tag not found.";

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { nodeListCase4, tagNameCase4, resultCase4 }
            };
        }

        public static IEnumerable<object[]> RemoveTagData()
        {
            //nodes already have tag
            var tagNameCase0 = "Tresearch SqlDAO Delete Tag1";
            var nodeListCase0 = new List<long> { 2022030536, 2022030537, 2022030538 };
            var resultCase0 = "200: Server: Tag removed from node(s).";

            //nodes do not contain these tags already
            var tagNameCase1 = "Tresearch SqlDAO Delete Tag2";
            var nodeListCase1 = new List<long> { 2022030536, 2022030537, 2022030538 };
            var resultCase1 = "200: Server: Tag removed from node(s).";

            // node doesn't already contain tag
            var tagNameCase2 = "Tresearch SqlDAO Delete Tag3";
            var nodeListCase2 = new List<long> { 2022030536 };
            var resultCase2 = "200: Server: Tag removed from node(s).";

            //Node already has tag
            var tagNameCase3 = "Tresearch SqlDAO Delete Tag4";
            var nodeListCase3 = new List<long> { };
            var resultCase3 = "200: Server: Tag removed from node(s).";

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 }
            };
        }

        public static IEnumerable<object[]> GetNodeTagData()
        {
            /**Nodes contain shared tags
             *      
             */
            var nodeListCase0 = new List<long> { 2022030539, 2022030540, 2022030541 };
            string expectedCase0 = "200: Server: Tag(s) retrieved.";
            var expectedTags0 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" };

            /**Nodes contain no shared tags
             * 
             */
            var nodeListCase1 = new List<long> { 2022030539, 2022030540, 2022030541, 2022030542 };
            string expectedCase1 = "200: Server: Tag(s) retrieved.";
            var expectedTags1 = new List<string> { };

            //Node has tags
            var nodeListCase2 = new List<long> { 2022030539 };
            string expectedCase2 = "200: Server: Tag(s) retrieved.";
            var expectedTags2 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3" };

            //Node contains no tags
            var nodeListCase3 = new List<long> { 2022030543 };
            string expectedCase3 = "200: Server: Tag(s) retrieved.";
            var expectedTags3 = new List<string> { };

            //No nodes passed in
            var nodeListCase4 = new List<long> {  };
            string expectedCase4 = "200: Server: Tag(s) retrieved.";
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
    }

    public class TagSqlDatabaseFixture : TestBaseClass, IDisposable
    {
        public TagSqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/DAOIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/DAOIntegrationCleanup.sql");

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
