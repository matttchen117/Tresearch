using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using System.Data;
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

        private static List<long> nodeids = new List<long>();
        public static List<long> NodeIDs
        {
            get { return nodeids; }
            set { nodeids = value; }
        }

        public SqlDAOShould(TagSqlDatabaseFixture fix) : base(new string[] { })
        {
            this.fixture = fix;                                     //Runs database startup script ONCE before tests. Runs database cleanup ONCE after tests.
            NodeIDs = fix.nodeIDs;
            TestProvider = TestServices.BuildServiceProvider();     //For Dependency Injection   
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

        /// <summary>
        ///     Test user adding tag to node(s)
        /// </summary>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated resoponse based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagAsync(List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            string result = await sqlDAO.AddTagAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test user creating a tag in tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="count">Count of nodes with tag</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
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

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test to delete tag name from tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(RemoveTagData))]
        public async Task RemoveTagAsync(string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.RemoveTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test to retreive a list of shared tags from a list of tag nodes
        /// </summary>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tags">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(List<int> index, List<string> tags, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            Tuple<List<string>, string> result = await sqlDAO.GetNodeTagsAsync(nodeIDs, cancellationTokenSource.Token);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result.Item2);
            Assert.Equal(tags, result.Item1);
        }

        /// <summary>
        ///     Tests to retrieve list of tags in tag bank
        /// </summary>
        /// <returns></returns>
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

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test to remove a tag from a list of node(s)
        /// </summary>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated response based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveNodeTagAsync(List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            string result = await sqlDAO.RemoveTagAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }


        /// <summary>
        /// Test data for adding tag to node(s)
        /// <br>Case 0: Tag nodes. Nodes exist and already contains tag.</br>
        /// <br>Case 1: Tag nodes. Nodes exist and DO NOT contain tag.</br>
        /// <br>Case 2: Tagging node. Node exist and does not contain tag.</br>
        /// <br>Case 3: Tagging node. Node exist and contains tag.</br>
        /// <br>Case 4: Tagging node. Node exist but tag doesn't exist in database.</br>
        /// <br>Case 5: Tagging node. Tag is null</br>
        /// <br>Case 6: Tagging node. Tag is empty string</br>
        /// <br>Case 7: Tagging node. Tag is string with just whitespace</br>
        /// <br>Case 8: Tagging node. List of node IDs is null</br>
        /// <br>Case 9: Tagging node. No nodes passed in</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> AddNodeTagData()
        {

            /**
             *  Case 0: Nodes exist and already contain tag 
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var case0 = new List<int> { 0, 1, 2 };
            var tagName0 = "Tresearch SqlDAO Add Tag1";
            var response0 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 1: Tagging node. Nodes exist and DO NOT contain tag 
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch SqlDAO Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var case1 = new List<int> { 0, 1, 2 };
            var tagName1 = "Tresearch SqlDAO Add Tag2";
            var response1 = IMessageBank.Responses.tagAddSuccess;


            /**
             *  Case 2: Tagging node. Node exist and does not contain tag 
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch SqlDAO Add Tag3
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var case2 = new List<int> { 0 };
            var tagName2 = "Tresearch SqlDAO Add Tag3";
            var response2 = IMessageBank.Responses.tagAddSuccess;


            /**
             *  Case 3: Tagging node. Node exist and contains tag 
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch SqlDAO Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var case3 = new List<int> { 0 };
            var tagName3 = "Tresearch SqlDAO Add Tag4";
            var response3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tagging node. Node exist but tag doesn't exist in database
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch SqlDAO Add Tag5
             *      
             *      Result:                     "404: Database: Tag not found."
             */
            var case4 = new List<int> { 0 };
            var tagName4 = "Tresearch SqlDAO Add Tag5";
            var response4 = IMessageBank.Responses.tagNotFound;

            /**
             *  Case 5: Tagging node. Tag is null
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   
             *      
             *      Result:                     "404: Database: Invalid tag."
             */
            var case5 = new List<int> { 0 };
            string tagName5 = null;
            var response5 = IMessageBank.Responses.tagNameInvalid;

            /**
            *  Case 6: Tagging node. Tag is empty string
            *      NodeIDs:                    xxxx0
            *      Tag Name:                   ""
            *      
            *      Result:                     "404: Database: Invalid tag."
            */
            var case6 = new List<int> { 0 };
            var tagName6 = "";
            var response6 = IMessageBank.Responses.tagNameInvalid;

            /**
            *  Case 7: Tagging node. Tag is string with just whitespace
            *      NodeIDs:                    xxxx0
            *      Tag Name:                   "     "
            *      
            *      Result:                     "404: Database: Invalid tag."
            */
            var case7 = new List<int> { 0 };
            var tagName7 = "     ";
            var response7 = IMessageBank.Responses.tagNameInvalid;


            /**
            *  Case 8: Tagging node. List of node IDs is null
            *      NodeIDs:                    
            *      Tag Name:                   Tresearch SqlDAO Add Tag1
            *      
            *      Result:                     "404: Database: Invalid tag."
            */
            List<int> case8 = null;
            var tagName8 = "Tresearch SqlDAO Add Tag1";
            var response8 = IMessageBank.Responses.nodeNotFound;

            /**
            *  Case 9: Tagging node. No nodes passed in
            *      NodeIDs:                    
            *      Tag Name:                   Tresearch SqlDAO Add Tag1
            *      
            *      Result:                     "404: Database: Invalid tag."
            */
            List<int> case9 = new List<int>();
            var tagName9 = "Tresearch SqlDAO Add Tag1";
            var response9 = IMessageBank.Responses.nodeNotFound;


            return new[]
            {
                new object[] { case0, tagName0, response0 },
                new object[] { case1, tagName1, response1 },
                new object[] { case2, tagName2, response2 },
                new object[] { case3, tagName3, response3 },
                new object[] { case4, tagName4, response4 },
                new object[] { case5, tagName5, response5 },
                new object[] { case6, tagName6, response6 },
                new object[] { case7, tagName7, response7 },
                new object[] { case8, tagName8, response8 },
                new object[] { case9, tagName9, response9 }
            };
        }


        /// <summary>
        /// Test Data to create tag in tag bank
        /// <br>Case 0: Tag does not exist.</br>
        /// <br>Case 1: Tag already exists in database.</br>
        /// <br>Case 2: Tag is null</br>
        /// <br>Case 3: Tag is empty string</br>
        /// <br>Case 4: Tag is string with just whitespace</br>
        /// <br>Case 5: Tag count is invalid (negative)</br>
        /// </summary>
        /// <returns></returns>
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

            /**
            *  Case 2: Tag is null
            *      Tag Name:                   null
            *      count:                      0
            */
            string tagNameCase2 = null;
            var count2 = 0;
            var resultCase2 = IMessageBank.Responses.tagNameInvalid;

            /**
            *  Case 3: Tag is empty string
            *      Tag Name:                   
            *      count:                      0
            */
            var tagNameCase3 = "";
            var count3 = 0;
            var resultCase3 = IMessageBank.Responses.tagNameInvalid;

            /**
            *  Case 4: Tag is string with just whitespace
            *      Tag Name:                   
            *      count:                      0
            */
            var tagNameCase4 = "   ";
            var count4 = 0;
            var resultCase4 = IMessageBank.Responses.tagNameInvalid;

            /**
            *  Case 5: Tag count is invalid (negative)
            *      Tag Name:                   DOES NOT MATTER
            *      count:                      -1
            */
            var tagNameCase5 = "DOES NOT MATTER";
            var count5 = -1;
            var resultCase5 = IMessageBank.Responses.tagCountInvalid;

            return new[]
           {
                new object[] { tagNameCase0, count0, resultCase0 },
                new object[] { tagNameCase1, count1, resultCase1 },
                new object[] { tagNameCase2, count2, resultCase2 },
                new object[] { tagNameCase3, count3, resultCase3 },
                new object[] { tagNameCase4, count4, resultCase4 },
                new object[] { tagNameCase5, count5, resultCase5 }
            };
        }


        /// <summary>
        ///     Test data to delete tag from tag bank.
        ///     <br>Case 0: Remove from tag bank. Tag exists.</br>
        ///     <br>Case 1: Remove from tag bank. Tag doesn't exist</br>
        ///     <br>Case 2: Remove from tag bank. Tag is null</br>
        ///     <br>Case 3: Remove from tag bank. Tag is empty</br>
        ///     <br>Case 4: Remove from tag bank. Tag is string with only whitespace characters</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> RemoveTagData()
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

            /*
             * Case 2: Remove from tag bank. Tag is null
             *      Tag Name:                   null
             */
            string tagNameCase2 = null;
            var resultCase2 = IMessageBank.Responses.tagNameInvalid;

            /*
             * Case 3: Remove from tag bank. Tag is empty
             *      Tag Name:                   null
             */
            var tagNameCase3 = "";
            var resultCase3 = IMessageBank.Responses.tagNameInvalid;

            /*
            * Case 4: Remove from tag bank. Tag is string with only whitespace characters
            *      Tag Name:                   null
            */
            var tagNameCase4 = "   ";
            var resultCase4 = IMessageBank.Responses.tagNameInvalid;


            return new[]
            {
                new object[] { tagNameCase0, resultCase0 },
                new object[] { tagNameCase1, resultCase1 },
                new object[] { tagNameCase2, resultCase2 },
                new object[] { tagNameCase3, resultCase3 },
                new object[] { tagNameCase4, resultCase4 }
            };
        }

        /// <summary>
        ///     Test data to retrieve list of shared tags from a list of node(s)
        ///     <br>Case 0: Nodes contains shared tags</br>
        ///     <br>Case 1: Nodes contain no shared tags</br>
        ///     <br>Case 2: Node has tags</br>
        ///     <br>Case 3: Node contains no tags</br>
        ///     <br>Case 4: No nodes passed in</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetNodeTagData()
        {
            /**
            *  Case 0:Nodes share common tags
            *      NodeIDs:                     [2022030539, 2022030540, 2022030541]
            *                                   2022030539: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3"
            *                                   2022030540: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" 
            *                                   2022030541: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2"
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            var case0 = new List<int>() { 7, 8, 9 };
            var tags0 = new List<string>() { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" };
            var expected0 = IMessageBank.Responses.tagGetSuccess;

            /**
            *  Case 1:Nodes do not share common tags
            *      NodeIDs:                     [2022030539, 2022030540, 2022030541]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            var case1 = new List<int>() {7, 8 , 9, 10 };
            var tags1 = new List<string>() { };
            var expected1 = IMessageBank.Responses.tagGetSuccess;

            /**
            *  Case 2: Node contains tags
            *      NodeIDs:                     [2022030543]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            var case2 = new List<int>() { 7 };
            var tags2 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3" };
            var expected2 = IMessageBank.Responses.tagGetSuccess;

            /**
            *  Case 3: Node contains no tags
            *      NodeIDs:                     [2022030543]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            var case3 = new List<int>() { 6 };
            var tags3 = new List<string> { };
            var expected3 = IMessageBank.Responses.tagGetSuccess;

            /**
           *  Case 4: No node passed in
           *      NodeIDs:                     []
           *      
           *      Result:                     "200: Server: Tag(s) retrieved."
           */
            var case4 = new List<int>() { };
            var tags4 = new List<string> { };
            var expected4 = IMessageBank.Responses.nodeNotFound;


            return new[]
            {
                new object[] { case0, tags0, expected0 },
                new object[] { case1, tags1, expected1 },
                new object[] { case2, tags2, expected2 },
                new object[] { case3, tags3, expected3 },
                new object[] { case4, tags4, expected4 }
            };
        }

        /// <summary>
        ///     Test data to remove tag from node.
        ///     <br>Case 0: Remove tag from node. Nodes exist and contain tags.</br>
        ///     <br>Case 1: Remove tag from node. Nodes exist but nodes do not contain tag.</br>
        ///     <br>Case 2: Remove tag from node. Node exist but doesn't contain tag</br>
        ///     <br>Case 3: Remove tag from node. No node passed in.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            //nodes already have tag
            var tagNameCase0 = "Tresearch SqlDAO Delete Tag1";
            var nodeListCase0 = new List<int> { 3, 4, 5 };
            var resultCase0 = IMessageBank.Responses.tagRemoveSuccess;

            //nodes do not contain these tags already
            var tagNameCase1 = "Tresearch SqlDAO Delete Tag2";
            var nodeListCase1 = new List<int> { 3, 4, 5 };
            var resultCase1 = IMessageBank.Responses.tagRemoveSuccess;

            // node doesn't already contain tag
            var tagNameCase2 = "Tresearch SqlDAO Delete Tag3";
            var nodeListCase2 = new List<int> { 3 };
            var resultCase2 = IMessageBank.Responses.tagRemoveSuccess;

            //Node already has tag
            var tagNameCase3 = "Tresearch SqlDAO Delete Tag4";
            var nodeListCase3 = new List<int> { 3 };
            var resultCase3 = IMessageBank.Responses.tagRemoveSuccess;

            return new[]
            {
                new object[] { nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { nodeListCase3, tagNameCase3, resultCase3 }
            };
        }
    }

    /// <summary>
    /// Fixture to run database setup and cleanup ONCE before and after all SqlDAO tests
    /// </summary>
    public class TagSqlDatabaseFixture : TestBaseClass, IDisposable
    {
        /// <summary>
        /// List of nodeIDs dynamically created
        /// </summary>
        public List<long> nodeIDs { get; set; }
        public TagSqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            //cleanup script location
            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/DAOIntegrationSetup.sql");

            //Read in commands
            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            // Setup database with cases
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
                var procedure = "dbo.[DAOIntegrationTagInitializeProcedure]";                        // Name of store procedure
                var value = new { };                                                                 // Parameters of stored procedure
                nodeIDs = connection.Query<long>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure)).ToList();
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
