using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    public class TagServiceShould : TestBaseClass, IClassFixture<TagServiceDatabaseFixture>
    {
        private TagServiceDatabaseFixture fixture;

        private static List<long> nodeids = new List<long>();
        private static List<long> NodeIDs
        {
            get { return nodeids; }
            set { nodeids = value; }
        }
        public TagServiceShould(TagServiceDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;                                 // Fixture initializes database once before all tests and after all tests
            NodeIDs = fixture.nodeIDs;                              // Nodeids (since nodes are created dynamically)
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<IMessageBank, MessageBank>();
            TestProvider = TestServices.BuildServiceProvider();
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
        public async Task AddTagToNodeAsync(List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            string result = await recoveryService.AddTagToNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test user removing tag from list of node(s)
        /// </summary>
        /// <param name="index">Indexes of node ID list</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated resoponse based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveTagFromNodeAsync(List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            string result = await recoveryService.RemoveTagFromNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        
        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetTagsFromNodesAsync( List<int> index, List<string> tags, IMessageBank.Responses response)
        {
            //Arrange
            ITagService tagService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            List<long> nodeIDs;
            if (index == null)
                nodeIDs = null;
            else
                nodeIDs = GetNodes(index);

            //Act
            Tuple<List<string>, string> results = await tagService.GetNodeTagsAsync(nodeIDs, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.NotNull(results);
            Assert.Equal(tags, results.Item1);        
            Assert.Equal(expected, results.Item2);        
        }


        /// <summary>
        ///     Tests user creating tag in tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="count">Number of nodes with tag</param>
        /// <param name="response">Expected enumerated resoponse based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTagAsync(string tagName, int count, IMessageBank.Responses response)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.CreateTagAsync(tagName, count, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Tests user deleting tag from tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="response">Expected enumerated resoponse based on case</param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(RemoveTagData))]
        public async Task RemoveTagAsync(string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await recoveryService.RemoveTagAsync(tagName, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///  Tests user or guest retrieving list of tags
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTagsAsync()
        {
            //Arrange
            ITagService tagService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> results = await tagService.GetTagsAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            string result = results.Item2;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
        
        /// <summary>
        ///     Test data to delete tag from tag bank.
        ///     <br>Case 0: Remove from tag bank. Tag exists.</br>
        ///     <br>Case 1: Remove from tag bank. Tag doesn't exist</br>
        ///     <br>Case 2: Remove from tag bank. Tag is null</br>
        ///     <br>Case 3: Remove from tag bank. Tag is empty</br>
        ///     <br>Case 4: Remove from tag bank. Tag is string with only whitespace characters</br>
        ///     <br>Case 5: Remove from tag bank. Nodes currently use tag</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> RemoveTagData()
        {
            /**
             *  Case 0: Remove from tag bank. Tag exists.  
             *      Tag Name:                   Tresearch Service Remove Me tag2
             *      
             *      Result:                     "200: Server: Tag removed from bank"
             */
            var tagNameCase0 = "Tresearch Service Remove Me tag2";
            var resultCase0 = IMessageBank.Responses.tagDeleteSuccess;

            /**
             *  Case 1: Remove from tag bank. Tag doesn't exist  
             *      Tag Name:                   Tresearch Service Remove Me tag1 
             *      
             *      Result:                     "200: Server: Tag removed from bank"
             */
            var tagNameCase1 = "Tresearch Service Remove Me tag1";
            var resultCase1 = IMessageBank.Responses.tagDeleteSuccess;

            /*
             * Case 2: Remove from tag bank. Tag is null
             *      Tag Name:                   null
             */
            string tagNameCase2 = null;
            var resultCase2 = IMessageBank.Responses.tagNameInvalid;

            /*
             * Case 2: Remove from tag bank. Tag is empty
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

            /*
            * Case 5: Remove from tag bank. Nodes currently use tag
            *      Tag Name:                   Tresearch Service Remove Me tag3
            */
            var tagNameCase5 = "Tresearch Service Remove Me tag3";
            var resultCase5 = IMessageBank.Responses.tagDeleteSuccess;


            return new[]
            {
                new object[] { tagNameCase0, resultCase0 },
                new object[] { tagNameCase1, resultCase1 },
                new object[] { tagNameCase2, resultCase2 },
                new object[] { tagNameCase3, resultCase3 },
                new object[] { tagNameCase4, resultCase4 },
                new object[] { tagNameCase5, resultCase5 }
            };
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {

            /**
            *  Case 0: Remove nodes tags. Nodes exist and contain tag. 
            *      NodeIDs:                    2022030533, 202203054, 2022030535
            *      Tag Name:                   Tresearch Service Delete Tag1
            *      
            *      Result:                     200: Server: Tag removed from node(s).
            */
            var tagCase0 = "Tresearch Service Delete Tag1";
            var nodeCase0 = new List<int> { 3, 4, 5 };
            var expected0 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 1: Remove nodes tags. Nodes exist and do not contain tags.
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                   Tresearch Service Delete Tag2
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase1 = "Tresearch Service Delete Tag2";
            var nodeCase1 = new List<int> { 3, 4,5 };
            var expected1 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 2: Remove nodes tags. Node exist and contain tag.
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag3
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase2 = "Tresearch Service Delete Tag3";
            var nodeCase2 = new List<int> { 3 };
            var expected2 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 3: Remove nodes tags. Node exist and does not contain tag contain tag.
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag4
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase3 = "Tresearch Service Delete Tag4";
            var nodeCase3 = new List<int> { 3 };
            var expected3 = IMessageBank.Responses.tagRemoveSuccess;

            /**
             *  Case 4: Remove nodes tags. No node passed in
             *      NodeIDs:                    
             *      Tag Name:                   Tresearch Service Delete Tag4
             *      
             *      Result:                     404: Database: The node was not found.
             */
            var tagCase4 = "Tresearch Service Delete Tag4";
            var nodeCase4 = new List<int> { };
            var expected4 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 5: Remove nodes tags. Node exist but tag doesn't exist
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag5
             *      
             *      Result:                     404: Database: The tag was not found.
             */
            var tagCase5 = "Tresearch Service Delete Tag5";
            var nodeCase5 = new List<int> { 3 };
            var expected5 = IMessageBank.Responses.tagRemoveSuccess;

            /**
            *  Case 6: Remove nodes tags. Some nodes already have  tag (0 has tag).
            *      NodeIDs:                    2022030530, 202203051, 2022030532
            *      Tag Name:                   Tresearch Service Add Tag3
            *      
            *      Result:                     200: Server: Tag removed from node(s).
            */
            var tagCase6 = "Tresearch Service Delete Tag3";
            var nodeCase6 = new List<int> { 3, 4, 5 };
            var expected6 = IMessageBank.Responses.tagRemoveSuccess;

            return new[]
            {
                new object[] { nodeCase0, tagCase0, expected0 },
                new object[] { nodeCase1, tagCase1, expected1 },
                new object[] { nodeCase2, tagCase2, expected2 },
                new object[] { nodeCase3, tagCase3, expected3 },
                new object[] { nodeCase4, tagCase4, expected4 },
                new object[] { nodeCase5, tagCase5, expected5 },
                new object[] { nodeCase6, tagCase6, expected6 }
            };
        }

        /// <summary>
        ///     Test data to add tag to node(s)
        ///     <br>Case 0: Tag nodes. Nodes exist and already contains tag.</br>
        ///     <br>Case 1: Tag nodes. Nodes exist and DO NOT contain tag.</br>
        ///     <br>Case 2: Tagging node. Node exist and contains tag.</br>
        ///     <br>Case 3: Tagging node. Node exist and does not contain tag.</br>
        ///     <br>Case 4: Tagging node. No node passed in</br>
        ///     <br>Case 5: Tagging node. Tag doesn't exist</br>
        ///     <br>Case 6: Tagging node. Some nodes already contain tag.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> AddNodeTagData()
        {
            /**
             * *Case 0: Tag nodes. Nodes exist and already contain tag. 
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                   Tresearch Service Add Tag1
             *
             *      Result:                     "200: Server: Tag added to node(s)."
              */
            var tagCase0 = "Tresearch Service Add Tag1";
            var nodeCase0 = new List<int> { 0, 1, 2 };
            var expected0 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 1: Tag nodes. Nodes exist and do not contain tag. 
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                  Tresearch Service Add Tag2
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase1 = "Tresearch Service Add Tag2";
            var nodeCase1 = new List<int> { 0, 1, 2 };
            var expected1 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 2: Tag node. Node exist and already contains tag.
             *      NodeIDs:                    2022030530
             *      Tag Name:                  Tresearch Service Add Tag3
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase2 = "Tresearch Service Add Tag3";
            var nodeCase2 = new List<int> { 0 };
            var expected2 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 3: Tag node. Node exist and does not contain tag.
             *      NodeIDs:                    2022030530
             *      Tag Name:                  Tresearch Service Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase3 = "Tresearch Service Add Tag4";
            var nodeCase3 = new List<int> { 0 };
            var expected3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tag node. No Node passed in.
             *      NodeIDs:                    
             *      Tag Name:                  Tresearch Service Add Tag4
             *      
             *      Result:                     "404: Database: Node not found."
             */
            var tagCase4 = "Tresearch Service Add Tag4";
            var nodeCase4 = new List<int> { };
            var expected4 = IMessageBank.Responses.nodeNotFound;

            /**
            *  Case 5: Tag node. Tag does not exist
            *      NodeIDs:                    2022030530
            *      Tag Name:                   Tresearch Service Add Tag5
            *      
            *      Result:                     "404: Database: Tag not found."
            */
            var tagCase5 = "Tresearch Service Add Tag55";
            var nodeCase5 = new List<int> { 0 };
            var expected5 = IMessageBank.Responses.tagNotFound;

            /**
            *  Case 6: Tag node. Some nodes already have  tag (0 has tag).
            *      NodeIDs:                    2022030530
            *      Tag Name:                   Tresearch Service Add Tag2
            *      
            *      Result:                     "200: Server: Tag added to node(s)."
            */
            var tagCase6 = "Tresearch Service Add Tag3";
            var nodeCase6 = new List<int> { 0, 1, 2 };
            var expected6 = IMessageBank.Responses.tagAddSuccess;

            return new[]
            {
                new object[] { nodeCase0, tagCase0, expected0 },
                new object[] { nodeCase1, tagCase1, expected1 },
                new object[] { nodeCase2, tagCase2, expected2 },
                new object[] { nodeCase3, tagCase3, expected3 },
                new object[] { nodeCase4, tagCase4, expected4 },
                new object[] { nodeCase5, tagCase5, expected5 },
                new object[] { nodeCase6, tagCase6, expected6 }
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
        /// 
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
            var tagNameCase0 = "Tresearch Service Create tag1";
            var count0 = 0;
            var resultCase0 = IMessageBank.Responses.tagCreateSuccess;

            /**
             *  Case 1: Tag already exists in database  
             *      Tag Name:                   "Tresearch Service Create tag2"
             *      count:                      0
             *      
             *      Result:                     "200: Server: Tag created in bank."
             */
            var tagNameCase1 = "Tresearch Service Create tag2";
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
        ///  Test data to retrieve shared tags from list of node(s).
        ///  <br>Case 0: Retrieve nodes tags. Nodes exist and contain shared tags. </br>
        ///  <br>Case 1: Retrieve nodes tags. Nodes exist and contains no shared tags.</br>
        ///  <br>Case 2: Retrieve nodes tags. Node exists and contains tag.</br>
        ///  <br>Case 3: Retrieve nodes tags. Node exists and contains no tag.</br>
        ///  <br>Case 4: Retrieve nodes tags. No nodes passed in</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetNodeTagData()
        {
            /**
             *  Case 0: Retrieve nodes tags. Nodes exist and contain shared tags. 
             *      NodeIDs:                    2072942636, 2072942637, 2072942638
             *      
             *                                  2072942636: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2', 'Tresearch Service Get Tag3'
             *                                  2072942637: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2'
             *                                  2072942638: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2'
             *                                  
             *      Shared tags:                Tresearch Service Get Tag1, Tresearch Service Get Tag2
             *      
             *      Result:                     200: Server: Tag(s) retrieved.
             */
            var nodeCase0 = new List<int> { 6, 7, 8 };
            var listCase0 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2" };
            var expected0 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 1: Retrieve nodes tags. Nodes exist and contains no shared tags. 
             *      NodeIDs:                    2072942639, 2072942640, 2072942641
             *      
             *                                  2072942639: 'Tresearch Service Get Tag1'
             *                                  2072942640: 'Tresearch Service Get Tag2'
             *                                  2072942641: 'Tresearch Service Get Tag3'
             *                                  
             *      Shared tags:                
             *      
             *      Result:                     200: Server: Tag(s) retrieved.
             */
            var nodeCase1 = new List<int> { 9, 10, 11 };
            var listCase1 = new List<string> { };
            var expected1 = IMessageBank.Responses.tagGetSuccess;


            /**
             *  Case 2: Retrieve nodes tags. Node exists and contains tag.
             *      NodeIDs:                    2072942636
             *      
             *                                  2072942636: 'Tresearch Service Get Tag1', 'Tresearch Service Get Tag2', 'Tresearch Service Get Tag3'
             *                                  
             *      Shared tags:                
             *      
             *      Result:                     200: Server: Tag(s) retrieved.
             */
            var nodeCase2 = new List<int> { 6 };
            var listCase2 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2", "Tresearch Service Get Tag3" };
            var expected2 = IMessageBank.Responses.tagGetSuccess;

            /**
             *  Case 3: Retrieve nodes tags. Node exists and contains no tag.
             *      NodeIDs:                    2072942642
             *      
             *                                  2072942642: 
             *                                  
             *      Shared tags:                
             *      
             *      Result:                     200: Server: Tag(s) retrieved.
             */
            var nodeCase3 = new List<int> { 12 };
            var listCase3 = new List<string> { };
            var expected3 = IMessageBank.Responses.tagGetSuccess;

            /**
            *  Case 4: Retrieve nodes tags. No nodes passed in
            *      NodeIDs:                                       
            *      Shared tags:                
            *      
            *      Result:                     404: Database: The node was not found.
            */
            var nodeCase4 = new List<int> { };
            var listCase4 = new List<string> { };
            var expected4 = IMessageBank.Responses.nodeNotFound;

            return new[]
           {
                new object[] { nodeCase0, listCase0, expected0 },
                new object[] { nodeCase1, listCase1, expected1 },
                new object[] { nodeCase2, listCase2, expected2 },
                new object[] { nodeCase3, listCase3, expected3 },
                new object[] { nodeCase4, listCase4, expected4 }
            };
        } 

    }

    public class TagServiceDatabaseFixture : TestBaseClass, IDisposable
    {
        public List<long> nodeIDs { get; set; }
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

            // Initialize list of nodes. 
            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                var procedure = "dbo.[ServiceIntegrationTagInitializeProcedure]";                        // Name of store procedure
                var value = new { };                                                                 // Parameters of stored procedure
                nodeIDs = connection.Query<long>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure)).ToList();
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
