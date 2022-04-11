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
        [InlineData("IntegrationRegistrationService1@gmail.com", "myRegisterPassword", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService2@gmail.com", "unFortunateName", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService3@gmail.com", "unFortunateName", "user", "409: Server: UserAccount  already exists")]
        public async Task CreateTheAccountAsync(string email, string passphrase, string authorizationLevel, string statusCode)
        {

            //Arrange
            IAccount account = new UserAccount(email, passphrase, "user", true, false);
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            string expected = statusCode;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<int, string> results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results.Item2);
        }


        [Theory]
        [InlineData("IntegrationRegistrationService4@gmail.com", "user", "200: Server: success")]
        [InlineData("IntegrationRegistrationService5@gmail.com", "user", "409: Database: The confirmation link already exists.")]
        [InlineData("IntegrationRegistrationService99@gmail.com", "user", "500: Database: The UserAccount was not found.")]
        public async Task CreateTheLinkAsync(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IConfirmationLink, string> results = await registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, results.Item2);        // GUID contains 36 characters
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

        public static IEnumerable<object[]> ConfirmData()
        {
            /**
             *  Case 0: Account exists. Not confirmed
             *      
             */
            var guid0 = "d3592438-07a9-462c-a6c7-db4b9b99cf45";
            var res0 = IMessageBank.Responses.generic;

            /**
             *  Case 1: Account exists. Already confirmed. 
             */
            var guid1 = "d3592438-07a9-462c-a6c7-db4b9b99cf43";
            var res1 = IMessageBank.Responses.confirmationLinkNotFound;

            /**
             *  Case 2: Account does not exist.
             */

            return new[]
            {
                new object[] { guid0, res0 },
                new object[] { guid1, res1 }
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
