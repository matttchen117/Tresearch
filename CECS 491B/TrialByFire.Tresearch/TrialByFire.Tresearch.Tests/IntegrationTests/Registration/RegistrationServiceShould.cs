using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Implementations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationServiceShould : TestBaseClass, IClassFixture<RegServiceDatabaseFixture>
    {
        RegServiceDatabaseFixture fixture;
        public RegistrationServiceShould(RegServiceDatabaseFixture  fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestProvider = TestServices.BuildServiceProvider();
        }


        

        [Theory]
        [MemberData(nameof(AccountData))]
        public async Task CreateTheAccountAsync(string email, string passphrase, string authorizationLevel, IMessageBank.Responses response)
        {

            //Arrange
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);  
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<int, string> results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results.Item2);
        }


        [Theory]
        [MemberData(nameof(ConfirmationLinkData))]
        public async Task CreateTheLinkAsync(string email, string authorizationLevel, IMessageBank.Responses response)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IConfirmationLink, string> results = await registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results.Item2);        // GUID contains 36 characters
        }

        

        [Theory]
        [MemberData(nameof(ConfirmData))]
        public async Task ConfirmTheUserAsync(string email, string authenticationLevel, IMessageBank.Responses response)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await registrationService.ConfirmAccountAsync(email, authenticationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetConfirmationLinkData))]
        public async Task GetConfirmationLink(string guid, IMessageBank.Responses response)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IConfirmationLink, string> link = await registrationService.GetConfirmationLinkAsync(guid, cancellationTokenSource.Token);
            string result = link.Item2;

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(HashValueData))]
        public async Task HashValue(string value, string expected)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await registrationService.HashValueAsync(value, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);

        }

        public static IEnumerable<object[]> AccountData()
        {
            // Account doesn't exist
            var username1 = "regServiceCreateUser1@tresearch.system";
            var passphrase1 = "test1";
            var role1 = "user";
            var expected1 = IMessageBank.Responses.generic;

            // Account already exists
            var username2 = "regServiceCreateUser2@tresearch.system";
            var passphrase2 = "test2";
            var role2 = "user";
            var expected2 = IMessageBank.Responses.accountAlreadyCreated;

            return new[]
            {
                new object[] { username1, passphrase1, role1,  expected1 },
                new object[] { username2, passphrase2, role2,  expected2 }
            };
        }

        public static IEnumerable<object[]> ConfirmationLinkData()
        {
            // Account exists, user, link doesnt already exist
            var username1 = "regServiceUser1@tresearch.system";
            var role1 = "user";
            var expected1 = IMessageBank.Responses.generic;

            // Account does not exist, already exists
            var username2 = "regServiceUser2@tresearch.system";
            var role2 = "user";
            var expected2 = IMessageBank.Responses.confirmationLinkExists;

            // Account does not exist
            var username3 = "regServiceUserDoesNotExist@gmail.com";
            var role3 = "user";
            var expected3 = IMessageBank.Responses.accountNotFound;

            //Account exists, user, link already exists

            //Account exists, admin,  link already exists 

            return new[]
            {
                new object[] { username1, role1,  expected1 },
                new object[] { username2, role2,  expected2 },
                new object[] { username3, role3, expected3 }
            };
        }

        public static IEnumerable<object[]> ConfirmData()
        {
            /**
             *  Case 0: Account exists. Not confirmed
             *      Account:
             *          Username: regServiceUserNotConfirmedEnabled@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            var user0 = "regServiceUserNotConfirmedEnabled@tresearch.system";
            var role0 = "user";
            var res0  = IMessageBank.Responses.generic;

            /**
             *  Case 1: Account exists. Already confirmed. 
             *      Account:
             *          Username: regServiceUserConfirmedEnabled@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: True
             */
            var user1 = "regServiceUserConfirmedEnabled@tresearch.system";
            var role1 = "user";
            var res1 = IMessageBank.Responses.generic;

            /**
             *  Case 2: Account does not exist.
             */
            var user2 = "regServiceAccountNotExist@tresearch.system";
            var role2 = "user";
            var res2 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { user0, role0, res0 },
                new object[] { user1, role1, res1 },
                new object[] { user2, role2, res2 }
            };
        }

        public static IEnumerable<object[]> GetConfirmationLinkData()
        {
            /**
             *  Case 0: Confirmation Link Exists
             *      Confirmation Link: d3592438-07a9-462c-a6c7-db4b9b99cf45
             *      Response: 200: Server: success
             */
            var guid0 = "d3592438-07a9-462c-a6c7-db4b9b99cf45";
            var res0 = IMessageBank.Responses.generic;

            /**
             *  Case 1: Confirmation Link does not exist
             *      Confirmation Link: d3592438-07a9-462c-a6c7-db4b9b99cf43
             *      Response: 200: Server: success
             */
            var guid1 = "d3592438-07a9-462c-a6c7-db4b9b99cf43";
            var res1 = IMessageBank.Responses.confirmationLinkNotFound;

            return new[]
            {
                new object[] { guid0, res0 },
                new object[] { guid1, res1 }
            };
        }

        public static IEnumerable<object[]> HashValueData()
        {
            //Correct
            string value0 = "pammypoor@gmail.com";
            string expected0 = "1D479F5F473B624F8DAE5A64BA677DAD94F0ED9C4B091D9B812B363B37BF070F3656867B3D3D4E318B04404DC2001F53E5DBA2069EF40C46C0DF77EF8FEF95A6";

            //Correct
            string value1 = "myPassphrase";
            string expected1 = "E355F3593633A62CB2DB3B66A3576688EBFAF5AAB904634EEEBF5093DACA30F0A8D734D3A844ADBD87D8A3160AAE51B0690085C94E9B34ADD2F9A5B9CCC10835";

            return new[]
            {
                new object[] { value0, expected0 },
                new object[] { value1, expected1 }
            };
        }
    }
    public class RegServiceDatabaseFixture : TestBaseClass, IDisposable
    {
        public RegServiceDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ServiceIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ServiceIntegrationCleanup.sql");

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
