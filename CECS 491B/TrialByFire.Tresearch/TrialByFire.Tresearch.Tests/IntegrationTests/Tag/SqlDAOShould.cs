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

        /**
         *  Case 0: Tag nodes. Nodes exist and already contains tag.
         *  Case 1: Tag nodes. Nodes exist and DO NOT contain tag.
         *  Case 2: Tagging node. Node exist and does not contain tag.
         *  Case 3: Tagging node. Node exist and contains tag.
         *  Case 4: Tagging node. Node exist but tag doesn't exist in database.
         *  Case 5: Tagging node. Node does not exist.
         */
        [Theory]
        [MemberData(nameof(AddTagData))]
        public async Task AddTagAsync(List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.AddTagAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTagAsync(string tagName, int count, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.CreateTagAsync(tagName, count, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }


        [Theory]
        [MemberData(nameof(DeleteData))]
        public async Task DeleteTagAsync(string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.DeleteTagAsync(tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }
        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(List<long> nodeIDs, IMessageBank.Responses response, List<string> expectedTags)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
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
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            string expected = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);
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
            /**
             *  Case 0: Nodes exist and already contain tag 
             *      NodeIDs:                    2022030533, 2022030534, 2022030535
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagNameCase0 = "Tresearch SqlDAO Add Tag1";
            var nodeListCase0 = new List<long> { 2022030533, 2022030534, 2022030535 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;


            /**
             *  Case 1: Tagging node. Nodes exist and DO NOT contain tag 
             *      NodeIDs:                    2022030533, 2022030534, 2022030535
             *      Tag Name:                   Tresearch SqlDAO Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagNameCase1 = "Tresearch SqlDAO Add Tag2";
            var nodeListCase1 = new List<long> { 2022030533, 2022030534, 2022030535 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 2: Tagging node. Node exist and does not contain tag 
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag3
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagNameCase2 = "Tresearch SqlDAO Add Tag3";
            var nodeListCase2 = new List<long> { 2022030533 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 3: Tagging node. Node exist and contains tag 
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagNameCase3 = "Tresearch SqlDAO Add Tag4";
            var nodeListCase3 = new List<long> { 2022030533 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tagging node. Node exist but tag doesn't exist in database
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag5
             *      
             *      Result:                     "404: Database: Tag not found."
             */
            var tagNameCase4 = "Tresearch SqlDAO Add Tag5";
            var nodeListCase4 = new List<long> { 2022030533 };
            var resultCase4 = IMessageBank.Responses.tagNotFound;

            /**
            *  Case 5: Tagging node. Node does not exist
            *      NodeIDs:                    2022030532
            *      Tag Name:                   Tresearch SqlDAO Add Tag5
            *      
            *      Result:                     "404: Database: The node was not found."
            */
            var tagNameCase5 = "Tresearch SqlDAO Add Tag5";
            var nodeListCase5 = new List<long> { 2022030532 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { nodeListCase5, tagNameCase5, resultCase5 }
            };
        }

        public static IEnumerable<object[]> CreateTagData()
        {
            /**
             *  Case 0: Tag does  not exist.  
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      count:                      0
             *      
             *      Result:                     "200: Server: Tag created in bank"
             */
            var tagNameCase0 = "Tresearch SqlDAO test tag1";
            var count0 = 0;
            var resultCase0 = IMessageBank.Responses.tagCreateSuccess;

            /**
             *  Case 1: Tag already exists in database  
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      count:                      0
             *      
             *      Result:                     "200: Server: Tag created in bank."
             */
            var tagNameCase1 = "Tresearch SqlDAO This Tag Exists Already";
            var count1 = 0;
            var resultCase1 = IMessageBank.Responses.tagDuplicate;

            return new[]
           {
                new object[] { tagNameCase0, count0, resultCase0 },
                new object[] { tagNameCase1, count1, resultCase1 }
            };
        }

        public static IEnumerable<object[]> DeleteData()
        {
            /**
             *  Case 0: Remove from tag bank. Tag exists.  
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      
             *      Result:                     "200: Server: Tag removed from bank"
             */
            var tagNameCase0 = "Tresearch SqlDAO Delete Me Tag";
            var resultCase0 = IMessageBank.Responses.tagDeleteSuccess;

            /**
             *  Case 1: Remove from tag bank. Tag doesn't exist  
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      
             *      Result:                     "200: Server: Tag removed from bank"
             */
            var tagNameCase1 = "Tresearch This Tag Doesnt exist";
            var resultCase1 = IMessageBank.Responses.tagDeleteSuccess;

            return new[]
           {
                new object[] { tagNameCase0, resultCase0 },
                new object[] { tagNameCase1, resultCase1 }
            };
        }

        public static IEnumerable<object[]> RemoveTagData()
        {
            /**
            *  Case 0: Remove tag from node. Nodes exist and contain tags.  
            *      Tag Name:                   Tresearch SqlDAO Delete Tag1
            *      NodeIDs:                     2022030536, 2022030537, 2022030538
            *      
            *      Result:                     "200: Server: Tag removed from bank"
            */
            var tagNameCase0 = "Tresearch SqlDAO Delete Tag1";
            var nodeListCase0 = new List<long> { 2022030536, 2022030537, 2022030538 };
            var resultCase0 = "200: Server: Tag removed from node(s).";

            /**
            *  Case 1: Remove tag from node. Nodes exist but nodes do not contain tags.  
            *      Tag Name:                   Tresearch SqlDAO Delete Tag1
            *      NodeIDs:                     2022030536, 2022030537, 2022030538
            *      
            *      Result:                     "200: Server: Tag removed from bank"
            */
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
            var expectedCase0 = IMessageBank.Responses.tagGetSuccess;
            var expectedTags0 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" };

            /**Nodes contain no shared tags
             * 
             */
            var nodeListCase1 = new List<long> { 2022030539, 2022030540, 2022030541, 2022030542 };
            var expectedCase1 = IMessageBank.Responses.tagGetSuccess;
            var expectedTags1 = new List<string> { };

            //Node has tags
            var nodeListCase2 = new List<long> { 2022030539 };
            var expectedCase2 = IMessageBank.Responses.tagGetSuccess; ;
            var expectedTags2 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3" };

            //Node contains no tags
            var nodeListCase3 = new List<long> { 2022030543 };
            var expectedCase3 = IMessageBank.Responses.tagGetSuccess;
            var expectedTags3 = new List<string> { };

            //No nodes passed in
            var nodeListCase4 = new List<long> {  };
            var expectedCase4 = IMessageBank.Responses.tagGetSuccess;
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
