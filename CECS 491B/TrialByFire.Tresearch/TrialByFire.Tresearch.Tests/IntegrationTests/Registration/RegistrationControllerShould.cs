using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationControllerShould : TestBaseClass, IClassFixture<RegConDatabaseFixture>
    {
        RegConDatabaseFixture fixture;
        public RegistrationControllerShould(RegConDatabaseFixture fixture) : base(new string[] { })
        {
            this.fixture  = fixture;
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<IRegistrationManager, RegistrationManager>();
            TestServices.AddScoped<IRegistrationController, RegistrationController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(CreateData))]
        public async Task RegisterTheUser(IRoleIdentity roleIdentity, string email, string passphrase, IMessageBank.Responses response)
        {
            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            string[] splitExpectation;
            splitExpectation = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]) };
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();

            //Act
            IActionResult results = await registrationController.RegisterAccountAsync(email, passphrase).ConfigureAwait(false); 
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }

        [Theory]
        [MemberData(nameof(ConfirmData))]
        public async void ConfirmTheUser(IRoleIdentity roleIdentity, string guid, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            string[] splitExpectation;
            splitExpectation = expected.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]) };
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();

            //Act
            IActionResult results = await registrationController.ConfirmAccountAsync(guid).ConfigureAwait(false);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }

        public static IEnumerable<object[]> CreateData()
        {
            /**
             *  Case 0: User is Authenticated. Account Exists. Confirmation link exists 
             *      Account:
             *          Username: RegManagerIntegration1@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "pammypoor+RegControllerIntegration1@gmail.system", "user", "79ea3f14c456d0b98fe4abc565420071ad32645ca2629923e5af830853242f545c3ae05c0ec046456e571a86ea5e72d87d7498c1e05852bd490c4486f6ded780");
            var username0 = "pammypoor+regControllerCreateUser2@gmail.com";
            var passphrase0 = "test1";
            var expected0 = IMessageBank.Responses.alreadyAuthenticated;

            /**
             *  Case 1: User is not authenticated. Account Exists 
             *      Account:
             *          Username: regControllerCreateUser2@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "guest", "guest");
            var username1 = "pammypoor+regControllerCreateUser2@gmail.com";
            var passphrase1 = "test1";
            var expected1 = IMessageBank.Responses.accountAlreadyCreated;

            /**
             *  Case 2: User is not authenticated. Account doesn't exist
             *      Account:
             *          Username: regControllerCreateUser1@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "guest", "guest");
            var username2 = "pammypoor+regControllerCreateUser1@gmail.com";
            var passphrase2 = "test1";
            var expected2 = IMessageBank.Responses.generic;

            return new[]
            {
                new object[] { roleIdentity0, username0, passphrase0, expected0 },
                new object[] { roleIdentity1, username1, passphrase1, expected1 },
                new object[] { roleIdentity2, username2, passphrase2, expected2 }
            };
        }


        public static IEnumerable<object[]> ConfirmData()
        {
            /**
             *  Case 0: User is Authenticated. Account Exists. Confirmation Link Exists
             *      Account:
             *          Username: RegManagerIntegration1@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "pammypoor+regControllerUserNotConfirmedEnabled@gmail.system", "user", "aa6ca100b849afcf9a93854936a4b223be5120565666a71c18f386bea7ec8eafee0f39f37f817b38968877132fbc480e5d226370788cae5d22e7338224af27d7");
            var guid0 = "A3592438-08a9-462c-a6cX-db4b9b99cf45";
            var expected0 = IMessageBank.Responses.alreadyAuthenticated;

            /**
             *  Case 1: User is not authenticated. Account Exists. Confirmation Link Exists
             *      Account:
             *          Username: regControllerCreateUser2@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "guest", "guest");
            var guid1 = "A3592438-08a9-462c-a6c7-db4b9b99cf45";
            var expected1 = IMessageBank.Responses.generic;

            /**
             *  Case 2: User is not authenticated. Account exists but is confirmed. ConfirmationLink exists
             *      Account:
             *          Username: regControllerUserConfirmedEnabled@tresearch.system
             *          AuthorizationLevel: user
             *          AccountStatus: True
             *          Confirmed: False
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "guest", "guest");
            var guid2 = "B3592438-08a9-462c-a6c7-db4b9b99cf45";
            var expected2 = IMessageBank.Responses.generic;

            return new[]
            {
                new object[] { roleIdentity0, guid0, expected0 },
                new object[] { roleIdentity1, guid1, expected1 },
                new object[] { roleIdentity2, guid2, expected2 }
            };
        }
    }

    public class RegConDatabaseFixture : TestBaseClass, IDisposable
    {
        public RegConDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ControllerIntegrationSetup.sql");

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

            string script = File.ReadAllText("../../../IntegrationTests/Registration/SetupAndCleanup/ControllerIntegrationCleanup.sql");

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
