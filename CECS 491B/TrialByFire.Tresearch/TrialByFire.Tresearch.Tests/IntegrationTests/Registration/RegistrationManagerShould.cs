using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationManagerShould : TestBaseClass, IClassFixture<RegManDatabaseFixture>
    {
        RegManDatabaseFixture fixture;
        public RegistrationManagerShould(RegManDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture = fixture;
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<IRegistrationManager, RegistrationManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(CreateData))]
        public async Task RegisterTheUser(IRoleIdentity roleIdentity, string email, string passphrase, string authorizationLevel, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IRegistrationManager registrationManager = TestProvider.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);

            //Act
            string results = await registrationManager.CreateAndSendConfirmationAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected , results);
        }

        public async Task ConfirmTheUser(string username, string guid, string statusCode)
        {
            //Arrange
            IRegistrationManager registrationManager = TestProvider.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationManager.ConfirmAccountAsync(guid, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        public void checkLinkInValidity(int minusHours)
        {
            //Arrange
            IRegistrationManager registrationManager = TestProvider.GetService<IRegistrationManager>();
            IConfirmationLink confirmationLink = new ConfirmationLink("test@gmail.com", "user", Guid.NewGuid(), DateTime.Now.AddHours(minusHours));
            bool expected;
            if (minusHours <= -24)
                expected = false;
            else
                expected = true;

            //Act
            bool actual = registrationManager.IsConfirmationLinkInvalid(confirmationLink);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CreateData()
        {
            /**
             *  Case 0: User is Authenticated. Account Exists 
             *      Account:
             *          Username: RegManagerIntegration1@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "pammypoor+RegManagerIntegration1@gmail.system", "user", "ceef03d444c0c33847d796ced4ed6cd95e89dbe64a9fbe7dcebcdb6b630d2c341d0c40ef3dcf365ab544ffa5a6fd6463d81a0c224f92fef88e7c4d084e2b6a1c");
            var username0 = "pammypoor+RegManagerIntegration1@gmail.system";
            var passphrase0 = "test1";
            var role0 = "user";
            var expected0 = IMessageBank.Responses.alreadyAuthenticated;

            /**
             *  Case 1: User is not authenticated. Account Exists 
             *      Account:
             *          Username: RegManagerIntegration1@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "guest", "guest");
            var username1 = "pammypoor+regManagerCreateUser2@gmail.com";
            var passphrase1 = "test1";
            var role1 = "user";
            var expected1 = IMessageBank.Responses.accountAlreadyCreated;

            return new[]
            {
                new object[] { roleIdentity0, username0, passphrase0, role0, expected0 },
                new object[] { roleIdentity1, username1, passphrase1, role1, expected1 }
            };
        }

    }

    public class RegManDatabaseFixture : TestBaseClass, IDisposable
    {
        public RegManDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ManagerIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ManagerIntegrationCleanup.sql");

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
