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


        /**
         *  Case 0: Tag nodes. Nodes exist and already contains tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 1: Tag nodes. Nodes exist and DO NOT contain tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 2: Tagging node. Node exist and contains tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 3: Tagging node. Node exist and does not contain tag.
         *      Result: "200: Server: Tag added to node(s)."
         *  Case 4: Tagging node. No node passed in
         *      Result: "404: Server: No node passed in.";
         *  Case 5: Tagging node. Tag doesn't exist
         *      Result: "404: Database: The tag was not found."
         *  Case 6: Tagging node. Some nodes already contain tag.
         *      Result: "200: Server: Tag added to node(s)."
         */
        [Fact]
        public async Task AddTagToNodeAsync()
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string success = await messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess);

            /**
             *  Case 0: Tag nodes. Nodes exist and already contain  tag. 
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                   Tresearch Service Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase0  = "Tresearch Service Add Tag1";
            var nodeCase0 = new List<long> { NodeIDs[0], NodeIDs[1], NodeIDs[2] };
            var expected0 = success;

            /**
             *  Case 1: Tag nodes. Nodes exist and do not contain tag. 
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                  Tresearch Service Add Tag2
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase1  = "Tresearch Service Add Tag2";
            var nodeCase1 = new List<long> { NodeIDs[0], NodeIDs[1], NodeIDs[2] };
            var expected1 = success;

            /**
             *  Case 2: Tag node. Node exist and already contains tag.
             *      NodeIDs:                    2022030530
             *      Tag Name:                  Tresearch Service Add Tag3
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase2 = "Tresearch Service Add Tag3";
            var nodeCase2 = new List<long> { NodeIDs[0] };
            var expected2 = success;

            /**
             *  Case 3: Tag node. Node exist and does not contain tag.
             *      NodeIDs:                    2022030530
             *      Tag Name:                  Tresearch Service Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase3 = "Tresearch Service Add Tag4";
            var nodeCase3 = new List<long> { NodeIDs[0] };
            var expected3 = success;

            /**
             *  Case 4: Tag node. No Node passed in.
             *      NodeIDs:                    
             *      Tag Name:                  Tresearch Service Add Tag4
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var tagCase4 = "Tresearch Service Add Tag4";
            var nodeCase4 = new List<long> { };
            var expected4 = await messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);

            /**
            *  Case 5: Tag node. No Node passed in.
            *      NodeIDs:                    2022030530
            *      Tag Name:                   Tresearch Service Add Tag5
            *      
            *      Result:                     "200: Server: Tag added to node(s)."
            */
            var tagCase5 = "Tresearch Service Add Tag5";
            var nodeCase5 = new List<long> { NodeIDs[0]};
            var expected5 = await messageBank.GetMessage(IMessageBank.Responses.tagNotFound);

            /**
            *  Case 6: Tag node. Some nodes already have  tag (0 has tag).
            *      NodeIDs:                    2022030530
            *      Tag Name:                   Tresearch Service Add Tag2
            *      
            *      Result:                     "200: Server: Tag added to node(s)."
            */
            var tagCase6 = "Tresearch Service Add Tag3";
            var nodeCase6 = new List<long> { NodeIDs[0], NodeIDs[1], NodeIDs[2] };
            var expected6 = success;


            //Act
            string result0 = await recoveryService.AddTagToNodesAsync(nodeCase0, tagCase0, cancellationTokenSource.Token).ConfigureAwait(false);
            string result1 = await recoveryService.AddTagToNodesAsync(nodeCase1, tagCase1, cancellationTokenSource.Token).ConfigureAwait(false);
            string result2 = await recoveryService.AddTagToNodesAsync(nodeCase2, tagCase2, cancellationTokenSource.Token).ConfigureAwait(false);
            string result3 = await recoveryService.AddTagToNodesAsync(nodeCase3, tagCase3, cancellationTokenSource.Token).ConfigureAwait(false);
            string result4 = await recoveryService.AddTagToNodesAsync(nodeCase4, tagCase4, cancellationTokenSource.Token).ConfigureAwait(false);
            string result5 = await recoveryService.AddTagToNodesAsync(nodeCase5, tagCase5, cancellationTokenSource.Token).ConfigureAwait(false);
            string result6 = await recoveryService.AddTagToNodesAsync(nodeCase6, tagCase6, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected0, result0);
            Assert.Equal(expected1, result1);
            Assert.Equal(expected2, result2);
            Assert.Equal(expected3, result3);
            Assert.Equal(expected4, result4);
            Assert.Equal(expected5, result5);
            Assert.Equal(expected6, result6);
        }
        
        [Fact]
        public async Task RemoveTagFromNodeAsync()
        {
            //Arrange
            ITagService recoveryService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string success = await messageBank.GetMessage(IMessageBank.Responses.tagRemoveSuccess);

            /**
             *  Case 0: Remove nodes tags. Nodes exist and contain tag. 
             *      NodeIDs:                    2022030533, 202203054, 2022030535
             *      Tag Name:                   Tresearch Service Delete Tag1
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase0  = "Tresearch Service Delete Tag1";
            var nodeCase0 = new List<long> { NodeIDs[3], NodeIDs[4], NodeIDs[5] };
            var expected0 = success;

            /**
             *  Case 1: Remove nodes tags. Nodes exist and do not contain tags.
             *      NodeIDs:                    2022030530, 202203051, 2022030532
             *      Tag Name:                   Tresearch Service Delete Tag2
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase1  = "Tresearch Service Delete Tag2";
            var nodeCase1 = new List<long> { NodeIDs[3], NodeIDs[4], NodeIDs[5] };
            var expected1 = success;

            /**
             *  Case 2: Remove nodes tags. Node exist and contain tag.
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag3
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase2 = "Tresearch Service Delete Tag3";
            var nodeCase2 = new List<long> { NodeIDs[3] };
            var expected2 = success;

            /**
             *  Case 3: Remove nodes tags. Node exist and does not contain tag contain tag.
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag4
             *      
             *      Result:                     200: Server: Tag removed from node(s).
             */
            var tagCase3  = "Tresearch Service Delete Tag4";
            var nodeCase3 = new List<long> { NodeIDs[3] };
            var expected3 = success;

            /**
             *  Case 4: Remove nodes tags. No node passed in
             *      NodeIDs:                    
             *      Tag Name:                   Tresearch Service Delete Tag4
             *      
             *      Result:                     404: Database: The node was not found.
             */
            var tagCase4 = "Tresearch Service Delete Tag4";
            var nodeCase4 = new List<long> { };
            var expected4 = await messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);

            /**
             *  Case 5: Remove nodes tags. Node exist but tag doesn't exist
             *      NodeIDs:                    2022030533
             *      Tag Name:                   Tresearch Service Delete Tag5
             *      
             *      Result:                     404: Database: The tag was not found.
             */
            var tagCase5 = "Tresearch Service Delete Tag5";
            var nodeCase5 = new List<long> { NodeIDs[3] };
            var expected5 = success;

            /**
            *  Case 6: Remove nodes tags. Some nodes already have  tag (0 has tag).
            *      NodeIDs:                    2022030530, 202203051, 2022030532
            *      Tag Name:                   Tresearch Service Add Tag3
            *      
            *      Result:                     200: Server: Tag removed from node(s).
            */
            var tagCase6 = "Tresearch Service Delete Tag3";
            var nodeCase6 = new List<long> { NodeIDs[3], NodeIDs[4], NodeIDs[5] };
            var expected6 = success;


            //Act
            string result0 = await recoveryService.RemoveTagFromNodesAsync(nodeCase0, tagCase0, cancellationTokenSource.Token).ConfigureAwait(false);
            string result1 = await recoveryService.RemoveTagFromNodesAsync(nodeCase1, tagCase1, cancellationTokenSource.Token).ConfigureAwait(false);
            string result2 = await recoveryService.RemoveTagFromNodesAsync(nodeCase2, tagCase2, cancellationTokenSource.Token).ConfigureAwait(false);
            string result3 = await recoveryService.RemoveTagFromNodesAsync(nodeCase3, tagCase3, cancellationTokenSource.Token).ConfigureAwait(false);
            string result4 = await recoveryService.RemoveTagFromNodesAsync(nodeCase4, tagCase4, cancellationTokenSource.Token).ConfigureAwait(false);
            string result5 = await recoveryService.RemoveTagFromNodesAsync(nodeCase5, tagCase5, cancellationTokenSource.Token).ConfigureAwait(false); 
            string result6 = await recoveryService.RemoveTagFromNodesAsync(nodeCase6, tagCase6, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected0, result0);
            Assert.Equal(expected1, result1);
            Assert.Equal(expected2, result2);
            Assert.Equal(expected3, result3);
            Assert.Equal(expected4, result4);
            Assert.Equal(expected5, result5);
            Assert.Equal(expected6, result6);
        }

        

        [Fact]
        public async Task GetTagsFromNodesAsync( )
        {
            //Arrange
            ITagService tagService = TestProvider.GetService<ITagService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string success = await messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

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
            var nodeCase0 = new List<long> { NodeIDs[6], NodeIDs[7], NodeIDs[8] };
            var listCase0 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2" };
            var expected0 = success;

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
            var nodeCase1 = new List<long> { NodeIDs[9], NodeIDs[10], NodeIDs[11] };
            var listCase1 = new List<string> {  };
            var expected1 = success;


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
            var nodeCase2 = new List<long> { NodeIDs[6] };
            var listCase2 = new List<string> { "Tresearch Service Get Tag1", "Tresearch Service Get Tag2", "Tresearch Service Get Tag3" };
            var expected2 = success;

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
            var nodeCase3 = new List<long> { NodeIDs[12] };
            var listCase3 = new List<string> { };
            var expected3 = success;

            /**
            *  Case 4: Retrieve nodes tags. No nodes passed in
            *      NodeIDs:                                       
            *      Shared tags:                
            *      
            *      Result:                     404: Database: The node was not found.
            */
            var nodeCase4 = new List<long> { };
            var listCase4 = new List<string> { };
            var expected4 = await messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);

            //Act

            Tuple<List<string>, string> results0 = await tagService.GetNodeTagsAsync(nodeCase0, cancellationTokenSource.Token);
            Tuple<List<string>, string> results1 = await tagService.GetNodeTagsAsync(nodeCase1, cancellationTokenSource.Token);
            Tuple<List<string>, string> results2 = await tagService.GetNodeTagsAsync(nodeCase2, cancellationTokenSource.Token);
            Tuple<List<string>, string> results3 = await tagService.GetNodeTagsAsync(nodeCase3, cancellationTokenSource.Token);
            Tuple<List<string>, string> results4 = await tagService.GetNodeTagsAsync(nodeCase4, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(listCase0, results0.Item1);        // Case 0: Assert tag list
            Assert.Equal(expected0, results0.Item2);        // Case 0: Assert string status

            Assert.Equal(listCase1, results1.Item1);        // Case 1: Assert tag list
            Assert.Equal(expected1, results1.Item2);        // Case 1: Assert string status

            Assert.Equal(listCase2, results2.Item1);        // Case 2: Assert tag list
            Assert.Equal(expected2, results2.Item2);        // Case 2: Assert string status

            Assert.Equal(listCase3, results3.Item1);        // Case 3: Assert tag list
            Assert.Equal(expected3, results3.Item2);        // Case 3: Assert string status

            Assert.Equal(listCase4, results4.Item1);        // Case 4: Assert tag list
            Assert.Equal(expected4, results4.Item2);        // Case 4: Assert string status

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
