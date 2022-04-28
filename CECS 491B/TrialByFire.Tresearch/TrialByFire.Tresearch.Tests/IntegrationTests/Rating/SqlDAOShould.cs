using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Rating
{
    public class SqlDAOShould: TestBaseClass, IClassFixture<RateSqlDatabaseFixture>
    {
        RateSqlDatabaseFixture fixture;
        public SqlDAOShould(RateSqlDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(RateData))]
        public async Task RateNode(string userHash, long nodeID, int rating, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.RateNodeAsync(userHash, nodeID, rating, cancellationTokenSource.Token);

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
            long nodeID0 = 5091676250;
            int rating0 = 1;
            IMessageBank.Responses messageBankResponse0 =  IMessageBank.Responses.userRateSuccess;

            /**
             *  Case 1: Rating invalid node. Account and user hash exists. 
             *      Username:                   "RateDAOUser1@gmail.com"
             *      AuthorizationLevel:         "user"
             *      User Hash:                  "a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834"
             *      NodeID:                     5091676249
             *      Rating:                     1
             *      
             *      Result:                     "200: Server: User rating added."
             * 
             */
            string userHash1 = "a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834";
            long nodeID1 = 5091676249;
            int rating1 = 1;
            IMessageBank.Responses messageBankResponse1 = IMessageBank.Responses.userRateFail;

            return new[]
            {
                new object[] { userHash0, nodeID0, rating0, messageBankResponse0 },
                new object[] { userHash1, nodeID1, rating1, messageBankResponse1 }
            };
        }
    }

    public class RateSqlDatabaseFixture : TestBaseClass, IDisposable
    {
        public RateSqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Rating/SetupAndCleanup/DAOIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Rating/SetupAndCleanup/DAOIntegrationCleanup.sql");

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
