using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;
using Dapper;
using System.Data;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Rate
{
    public class SqlDAOShould: TestBaseClass, IClassFixture<RateSqlDatabaseFixture>
    {
        RateSqlDatabaseFixture fixture;
        public SqlDAOShould(RateSqlDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            NodeIDs = fixture.nodeIDs;
            TestProvider = TestServices.BuildServiceProvider();
        }

        private static List<long> nodeids = new List<long>();
        public static List<long> NodeIDs
        {
            get { return nodeids; }
            set { nodeids = value; }
        }

        /// <summary>
        /// Retrieves list of nodeids matching indices.
        /// </summary>
        /// <param name="indexes">Indices of return nodeIDs</param>
        /// <returns></returns>
        public long GetNode(int id)
        {
            return nodeids[id];
        }

        public List<long> GetNodes(List<int> indexes)
        {
            List<long> ids = new List<long>();
            foreach (int index in indexes)
                ids.Add(NodeIDs[index]);
            return ids;
        }

        [Theory]
        [MemberData(nameof(RateData))]
        public async Task RateNode(string userHash, int nodeID, int rating, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string ex = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            NodeRating r = new NodeRating(userHash, GetNode(nodeID), rating);
            
            string[] split;
            split = ex.Split(":");

            IResponse<NodeRating> expected;

            if(Convert.ToInt32(split[0]) == 200)
            {
                expected = new RateResponse<NodeRating>("", r, 200, true);
            }
            else
            {
                expected = new RateResponse<NodeRating>(ex, r, Convert.ToInt32(split[0]), false);
            }
            //Act
            IResponse<NodeRating> result = await sqlDAO.RateNodeAsync(r, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        [MemberData(nameof(GetRatingsData))]
        public async Task GetRatings(List<int> ids, double ratings, IEnumerable<Node> nodes, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            List<long> nodeIDs = GetNodes(ids);

            for(int i = 0; i < nodes.Count(); i++)
            {
                nodes.ElementAt(i).NodeID = nodeIDs[i];
            }

            string ex = await messageBank.GetMessage(response);
            string[] split;
            split = ex.Split(":");

            IResponse<IEnumerable<Node>> expected;

            if (response.Equals(IMessageBank.Responses.getRateSuccess))
            {
                expected = new RateResponse<IEnumerable<Node>>("", nodes, 200, true);
            }
            else
            {
                expected = new RateResponse<IEnumerable<Node>>(ex, nodes, Convert.ToInt32(split[0]), false);
            }


            //Act
            IResponse<IEnumerable<Node>> result = await sqlDAO.GetNodeRatingAsync(nodeIDs);

            //Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> RateData()
        {
            /**
             *  Case 0: Rating valid node.Account and user hash exists.
             *      Username:                   "RateDAOUser1@gmail.com"
             *      AuthorizationLevel:         "user"
             *      User Hash:                  "a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834"
             *      NodeID:                     5091676250
             *      Rating:                     1
             *      
             *      Result:                     "200: Server: User rating added."
             * 
             */
            string userHash0 = "a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834";
            var nodeID0 = 0;
            var rating0 = 1;
            IMessageBank.Responses messageBankResponse0 =  IMessageBank.Responses.userRateSuccess;


            return new[]
            {
                new object[] { userHash0, nodeID0, rating0, messageBankResponse0 }
            };
        }

        public static IEnumerable<object[]> GetRatingsData()
        {
            /**
             *  Case 0: Rating valid node.Account and user hash exists.
             *      Username:                   "RateDAOUser1@gmail.com"
             *      AuthorizationLevel:         "user"
             *      User Hash:                  "a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834"
             *      NodeID:                     5091676250
             *      Rating:                     1
             *      
             *      Result:                     "200: Server: User rating added."
             * 
             */
            var nodeID0 = new List<int> { 0, 1 };
            var ratings0 = 1.0;
            IMessageBank.Responses messageBankResponse0 = IMessageBank.Responses.getRateSuccess;

            return new[]
            {
                new object[] { nodeID0, ratings0, messageBankResponse0 }
            };
        }
    }

    public class RateSqlDatabaseFixture : TestBaseClass, IDisposable
    {
        public List<long> nodeIDs { get; set; }
        public RateSqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Rate/SetupAndCleanup/DAOIntegrationSetup.sql");

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
                var procedure = "dbo.[DAOIntegrationRateInitializeProcedure]";                        // Name of store procedure
                var value = new { };                                                                 // Parameters of stored procedure
                nodeIDs = connection.Query<long>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public void Dispose()
        {
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Rate/SetupAndCleanup/DAOIntegrationCleanup.sql");

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
