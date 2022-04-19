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
            TestProvider = TestServices.BuildServiceProvider();         //For Dependency Injection   
        }

        /**
         *  Case 0: Tag nodes. Nodes exist and already contains tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 1: Tag nodes. Nodes exist and DO NOT contain tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 2: Tagging node. Node exist and does not contain tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 3: Tagging node. Node exist and contains tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 4: Tagging node. Node exist but tag doesn't exist in database.
         *      Result: "404: Database: Tag not found."
         *  Case 5: Tagging node. Node does not exist.
         *      Result: "404: Database: The node was not found."
         */

        [Fact]
        public async Task AddTagAsync()
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            /**
             *  Case 0: Nodes exist and already contain tag 
             *      NodeIDs:                    2022030533, 2022030534, 2022030535
             *      Tag Name:                   Tresearch SqlDAO Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            List<long> case0 = new List<long> { NodeIDs[0], NodeIDs[1], NodeIDs[2] };
            string tagCase00 = "Tresearch SqlDAO Add Tag1";

            /**
             *  Case 1: Tagging node. Nodes exist and DO NOT contain tag 
             *      NodeIDs:                    2022030533, 2022030534, 2022030535
             *      Tag Name:                   Tresearch SqlDAO Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            List<long> case1 = new List<long> { NodeIDs[0], NodeIDs[1], NodeIDs[2] };
            string tagCase01 = "Tresearch SqlDAO Add Tag2";


            /**
             *  Case 2: Tagging node. Node exist and does not contain tag 
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag3
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            List<long> case2 = new List<long> { NodeIDs[0] };
            string tagCase02 = "Tresearch SqlDAO Add Tag3";


            /**
             *  Case 3: Tagging node. Node exist and contains tag 
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            List<long> case3 = new List<long> { NodeIDs[0] };
            string tagCase03 = "Tresearch SqlDAO Add Tag4";

            /**
             *  Case 4: Tagging node. Node exist but tag doesn't exist in database
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch SqlDAO Add Tag5
             *      
             *      Result:                     "404: Database: Tag not found."
             */
            List<long> case4 = new List<long> { NodeIDs[0] };
            string tagCase04 = "Tresearch SqlDAO Add Tag5";

            /**
            *  Case 5: Tagging node. Node does not exist
            *      NodeIDs:                    node id that doesn't exist
            *      Tag Name:                   Tresearch SqlDAO Add Tag5
            *      
            *      Result:                     "404: Database: The node was not found."
            */
            List<long> case5 = new List<long> { -1 };
            string tagCase05 = "Tresearch SqlDAO Add Tag5";

            //Act
            string resultCase0 = await sqlDAO.AddTagAsync(case0, tagCase00, cancellationTokenSource.Token);
            string resultCase1 = await sqlDAO.AddTagAsync(case1, tagCase01, cancellationTokenSource.Token);
            string resultCase2 = await sqlDAO.AddTagAsync(case2, tagCase02, cancellationTokenSource.Token);
            string resultCase3 = await sqlDAO.AddTagAsync(case3, tagCase03, cancellationTokenSource.Token);
            string resultCase4 = await sqlDAO.AddTagAsync(case4, tagCase04, cancellationTokenSource.Token);
            string resultCase5 = await sqlDAO.AddTagAsync(case5, tagCase05, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess), resultCase0);
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess), resultCase1);
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess), resultCase2);
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess), resultCase3);
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagNotFound), resultCase4);
            Assert.Equal(await messageBank.GetMessage(IMessageBank.Responses.tagNotFound), resultCase5);
        }

        /**
         *  Case 0: Tag does not exist.
         *      Result: ""200: Server: Tag created in bank"
         *  Case 1: Tag already exists in database.
         *      Result: "409: Database: The tag already exists."
         */
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
            Assert.Equal(expected, result);
        }

        /**
         *  Case 0: Remove from tag bank. Tag exists.
         *      Result: "200: Server: Tag removed from bank"
         *  Case 1: Tag already exists in database.
         *      Result: "200: Server: Tag removed from bank"
         */
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

            //Assert
            Assert.Equal(expected, result);
        }

        /**
         *  Case 0: Nodes contains shared tags
         *      Result: "200: Server: Tag(s) retrieved."
         *  Case 1: Nodes contain no shared tags
         *      Result: "200: Server: Tag(s) retrieved."
         *  Case 2: Node has tags
         *      Result: "200: Server: Tag(s) retrieved."
         *  Case 3: Node contains no tags
         *      Result: "200: Server: Tag(s) retrieved."
         *  Case 4: No nodes passed in
         *      Result: "200: Server: Tag(s) retrieved."
         */


        [Fact]
        public async Task GetNodeTagsAsync()
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            /**
            *  Case 0:Nodes share common tags
            *      NodeIDs:                     [2022030539, 2022030540, 2022030541]
            *                                   2022030539: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3"
            *                                   2022030540: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" 
            *                                   2022030541: "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2"
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            List<long> case0 = new List<long>() { NodeIDs[7], NodeIDs[8], NodeIDs[9] };
            List<string> tags0 = new List<string>() { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2" };
            string expected0 = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            /**
            *  Case 1:Nodes do not share common tags
            *      NodeIDs:                     [2022030539, 2022030540, 2022030541]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            List<long> case1 = new List<long>() { NodeIDs[7], NodeIDs[8], NodeIDs[9], NodeIDs[10]};
            List<string> tags1 = new List<string>() {  };
            string expected1 = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            /**
            *  Case 2: Node contains tags
            *      NodeIDs:                     [2022030543]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            List<long> case2 = new List<long>() { NodeIDs[7] };
            List<string> tags2 = new List<string> { "Tresearch SqlDAO Get Tag1", "Tresearch SqlDAO Get Tag2", "Tresearch SqlDAO Get Tag3" };
            string expected2 = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            /**
            *  Case 3: Node contains no tags
            *      NodeIDs:                     [2022030543]
            *      
            *      Result:                     "200: Server: Tag(s) retrieved."
            */
            List<long> case3 = new List<long>() { NodeIDs[6] };
            List<string> tags3 = new List<string> { };
            string expected3 = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            /**
           *  Case 4: No node passed in
           *      NodeIDs:                     []
           *      
           *      Result:                     "200: Server: Tag(s) retrieved."
           */
            List<long> case4 = new List<long>() { };
            List<string> tags4 = new List<string> { };
            string expected4 = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);

            //Act
            Tuple<List<string>, string> result0 = await sqlDAO.GetNodeTagsAsync(case0, cancellationTokenSource.Token);
            Tuple<List<string>, string> result1 = await sqlDAO.GetNodeTagsAsync(case1, cancellationTokenSource.Token);
            Tuple<List<string>, string> result2 = await sqlDAO.GetNodeTagsAsync(case2, cancellationTokenSource.Token);
            Tuple<List<string>, string> result3 = await sqlDAO.GetNodeTagsAsync(case3, cancellationTokenSource.Token);
            Tuple<List<string>, string> result4 = await sqlDAO.GetNodeTagsAsync(case4, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected0, result0.Item2);
            Assert.Equal(tags0, result0.Item1);
            
            Assert.Equal(expected1, result1.Item2);
            Assert.Equal(tags1, result1.Item1);
            
            Assert.Equal(expected2, result2.Item2);
            Assert.Equal(tags2, result2.Item1);

            Assert.Equal(expected3, result3.Item2);
            Assert.Equal(tags3, result3.Item1);

            Assert.Equal(expected4, result4.Item2);
            Assert.Equal(tags4, result4.Item1);
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

            //Assert
            Assert.Equal(expected, result);
        }


        /**
        *  Case 0: Remove tag from node. Nodes exist and contain tags.
        *      Result:  "200: Server: Tag removed from node(s)."
        *  Case 1: Remove tag from node. Nodes exist but nodes do not contain tag.
        *      Result: "200: Server: Tag removed from node(s)."
        *  Case 2: Remove tag from node. Node exist but doesn't contain tag
        *      Result:  "200: Server: Tag removed from node(s)."
        *  Case 3: Remove tag from node. No node passed in.
        *      Result: "200: Server: Tag removed from node(s)."
        */
        [Fact]
        public async Task RemoveTagAsync()
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string success = await messageBank.GetMessage(IMessageBank.Responses.tagRemoveSuccess);

            /**
            *  Case 0: Remove tag from node. Nodes exist and contain tags.  
            *      Tag Name:                   Tresearch SqlDAO Delete Tag1
            *      NodeIDs:                     2022030536, 2022030537, 2022030538
            *      
            *      Result:                     "200: Server: Tag removed from node(s)."
            */
            List<long> case0 = new List<long> { NodeIDs[3], NodeIDs[4], NodeIDs[5] };
            string tagCase0  = "Tresearch SqlDAO Delete Tag1";
            string expected0 = success;

            //Act
            string result0 = await sqlDAO.RemoveTagAsync(case0, tagCase0, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected0, result0);
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
