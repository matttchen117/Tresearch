using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class SqlDAOShould: TestBaseClass, IClassFixture<RecoverySqlDatabaseFixture>
    {
        RecoverySqlDatabaseFixture fixture;
        public SqlDAOShould(RecoverySqlDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestProvider = TestServices.BuildServiceProvider();         //For Dependency Injection
        }

        [Theory]
        [MemberData(nameof(GetRecoveryLinkCountData))]
        public async Task GetUserRecoveryLinkCountAsync(string username, string authorizationLevel, int expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            int result = await sqlDAO.GetRecoveryLinkCountAsync(username, authorizationLevel, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task CreateRecoveryLinkAsync(IRecoveryLink link, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result  = await sqlDAO.CreateRecoveryLinkAsync(link, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task GetUserRecoveryLinkAsync(string guid, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            Tuple<IRecoveryLink, string> results = await sqlDAO.GetRecoveryLinkAsync(guid, cancellationTokenSource.Token);
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task EnableUserAccountAsync(string username, string authorizationLevel, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result = await sqlDAO.EnableAccountAsync(username, authorizationLevel, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task RemoveUserRecoveryLinkAsync(IRecoveryLink recoveryLink, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result = await sqlDAO.RemoveRecoveryLinkAsync(recoveryLink, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public async Task IncrementUserRecoveryLinkCountAsync(string username, string authorizationLevel, IMessageBank.Responses response)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result = await sqlDAO.IncrementRecoveryLinkCountAsync(username, authorizationLevel, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetRecoveryLinkCountData()
        {
            /**
             *  Case 0: Account exists. Account already has recovery link count
             *      Account:                    
             *          Username:   sqlShouldRegUser1@tresearch.systems      
             *          Role:       user
             *          Status:     disabled
             *          Count: 2
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            var user0 =  "sqlShouldRegUser1@tresearch.systems";
            var role0 =  "user";
            var count0 = 2;

            return new[]
           {
                new object[] { user0, role0, count0 }
            };
        }

    }

    public class RecoverySqlDatabaseFixture : TestBaseClass, IDisposable
    {
        public IList<long> nodeIDs { get; set; }
        public RecoverySqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Recovery/SetupAndCleanup/DAOIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Recovery/SetupAndCleanup/DAOIntegrationCleanup.sql");

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
