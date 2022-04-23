using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class SqlDAOShould : TestBaseClass, IClassFixture<RegSqlDatabaseFixture>
    {
        RegSqlDatabaseFixture fixture;
        public SqlDAOShould(RegSqlDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AccountData))]
        public async Task CreateAccountAsync(IAccount account, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<int, string> result = await sqlDAO.CreateAccountAsync(account, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result.Item2);
        }

        [Theory]
        [MemberData(nameof(OTPData))]
        public async Task CreateOTPClaim(string username, string authorizationLevel, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await sqlDAO.CreateOTPAsync(username, authorizationLevel, 0, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ConfirmationLinkData))]
        public async Task CreateConfirmationLink(IConfirmationLink confirmationLink, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await sqlDAO.CreateConfirmationLinkAsync(confirmationLink, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ConfirmData))]
        public async Task ConfirmAccountAsync(string username, string authorizationLevel, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await sqlDAO.UpdateAccountToConfirmedAsync(username, authorizationLevel, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }


        public static IEnumerable<object[]> AccountData()
        {
            // Create user
            IAccount account1 = new UserAccount("sqlDAORegUser1@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected1 = "200: Server: success";

            //Create User, account already exists
            IAccount account2 = new UserAccount("sqlDAORegUser2@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected2 = "409: Server: UserAccount  already exists";

            // Create Admin
            IAccount account3 = new UserAccount("sqlDAORegAdmin1@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected3 = "200: Server: success";

            //Create Admin, account already exists
            IAccount account4 = new UserAccount("sqlDAORegAdmin2@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "admin", true, false);
            string expected4 = "409: Server: UserAccount  already exists";

            return new[]
            {
                new object[] { account1, expected1  },
                new object[] { account2, expected2  },
                new object[] { account3, expected3  },
                new object[] { account4, expected4  }
            };
        }

        public static IEnumerable<object[]> OTPData()
        {
            // Account exists, user, otp doesnt already exist
            string username1 = "sqlDAORegOTPUser1@gmail.com";
            string role1 = "user";
            string expected1 = "200: Server: success";


            // Account exists, admin, otp doesn't already exist
            string username2 = "sqlDAORegOTPAdmin1@gmail.com";
            string role2 = "admin";
            string expected2 = "200: Server: success";

            // Account does not exist, user
            string username3 = "sqlDAORegOTPUser2@gmail.com";
            string role3 = "user";
            string expected3 = "500: Database: The UserAccount was not found.";

            // Account does not exist, admin
            string username4 = "sqlDAORegOTPAdmin2@gmail.com";
            string role4 = "admin";
            string expected4 = "500: Database: The UserAccount was not found.";

            //Account exists, user, otp already exists

            //Account exists, admin,  otp already exists 

            return new[]
            {
                new object[] { username1, role1, expected1 },
                new object[] { username2, role2, expected2 },
                new object[] { username3, role3, expected3 },
                new object[] { username4, role4, expected4 }
            };

        }


        public static IEnumerable<object[]> ConfirmationLinkData()
        {
            // Account exists, user, link doesnt already exist
            IConfirmationLink link1 = new ConfirmationLink("sqlDAORegConUser1@gmail.com", "user", Guid.NewGuid(), DateTime.Now);
            string expected1 = "200: Server: success";

            // Account exists, admin, link doesn't already exist
            IConfirmationLink link2 = new ConfirmationLink("sqlDAORegConAdmin1@gmail.com", "admin", Guid.NewGuid(), DateTime.Now);
            string expected2 = "200: Server: success";

            // Account does not exist, user
            IConfirmationLink link3 = new ConfirmationLink("sqlDAORegConUser2@gmail.com", "user", Guid.NewGuid(), DateTime.Now);
            string expected3 = "500: Database: The UserAccount was not found.";

            // Account does not exist, admin
            IConfirmationLink link4 = new ConfirmationLink("sqlDAORegConAdmin2@gmail.com", "admin", Guid.NewGuid(), DateTime.Now);
            string expected4 = "500: Database: The UserAccount was not found.";

            //Account exists, user, link already exists

            //Account exists, admin,  link already exists 

            return new[]
            {
                new object[] { link1, expected1 },
                new object[] { link2, expected2 },
                new object[] { link3, expected3 },
                new object[] { link4, expected4 }
            };
        }

        public static IEnumerable<object[]> ConfirmData()
        {
            // Account exists, user, account not confirmed
            string username1 = "sqlDAOConfirmUser1@gmail.com";
            string role1 = "user";
            string expected1 = "200: Server: success";


            // Account exists, admin, account not confirmed
            string username2 = "sqlDAOConfirmAdmin1@gmail.com";
            string role2 = "admin";
            string expected2 = "200: Server: success";

            // Account does not exist, user
            string username3 = "sqlDAOConfirmUser2@gmail.com";
            string role3 = "user";
            string expected3 = "500: Database: The UserAccount was not found.";

            // Account does not exist, admin
            string username4 = "sqlDAOConfirmAdmin2@gmail.com";
            string role4 = "admin";
            string expected4 = "500: Database: The UserAccount was not found.";

            //Account exists, user, account already confirmed

            //Account exists, admin, account already confirmed

            return new[]
            {
                new object[] { username1, role1, expected1 },
                new object[] { username2, role2, expected2 },
                new object[] { username3, role3, expected3 },
                new object[] { username4, role4, expected4 }
            };
        }
    }
    public class RegSqlDatabaseFixture : TestBaseClass, IDisposable
    {
        public RegSqlDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/DAOIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/DAOIntegrationCleanup.sql");

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