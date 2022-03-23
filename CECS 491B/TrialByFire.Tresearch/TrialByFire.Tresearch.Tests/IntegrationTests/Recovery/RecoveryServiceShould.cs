using Xunit;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class RecoveryServiceShould : IntegrationTestDependencies
    {
        public RecoveryServiceShould() : base() { }

        [Theory]
        [InlineData("IntegrationRecoveryService1@gmail.com", "200: Server: success", "user")]
        [InlineData("IntegrationRecoveryService2@gmail.com", "200: Server: success", "user")]
        public async Task GetAccountAsync(string username, string statusCode, string authorizationLevel)
        {
            //Arrange
            IRecoveryService recoveryService = new RecoveryService(SqlDAO, LogService, MessageBank);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expectedStatusCode = statusCode;


            //Act
            Tuple<IAccount, string> results = await recoveryService.GetAccountAsync(username, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);
            string resultStatusCode = results.Item2;
            
            //Assert
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService1@gmail.com", "myPassphrase", "user", false, true, "200: Server: success")]
        [InlineData("IntegrationRecoveryService2@gmail.com", "aValidPassphrase", "admin", true, false, "200:Server: success")]
        [InlineData("IntegrationRecoveryService3@gmail.com", "doesnTMatTer", "admin", null, false, "404: Database: The account was not found.")]
        public async Task CheckAccountStatusAsync(string username, string passphrase, string authorizationLevel, bool accountStatus, bool confirmed, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = new RecoveryService(SqlDAO, LogService, MessageBank);
            IAccount account = new Account(username, username, passphrase, authorizationLevel, accountStatus, confirmed);
            bool expected;
            if (accountStatus == null)
                expected = false;
            else
                expected = !accountStatus;
            string expectedStatusCode = statusCode;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));


            //Act
            Tuple<bool, string> results = await recoveryService.IsAccountDisabledAsync(account, cancellationTokenSource.Token).ConfigureAwait(false);
            bool result = results.Item1;
            string resultStatusCode = results.Item2;

            //Assert
            Assert.Equal(expected, result);
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        [Theory]    //Change these to match database!
        [InlineData("IntegrationRecoveryService3@gmail.com", "myPassphrase", "200: Server: success", "user", false, true)]   //Account already has a link
        [InlineData("IntegrationRecoveryService4@gmail.com", "myPassphrase", "200: Server: success", "user", false, true)]   //Account does not have a link
        public async Task CreateALinkAsync(string username, string passphrase, string statusCode, string authorizationLevel, bool accountStatus, bool confirmed)
        {
            //Arrange
            IRecoveryService recoveryService = new RecoveryService(SqlDAO, LogService, MessageBank);
            IAccount account = new Account(username, username, passphrase, authorizationLevel, accountStatus, confirmed);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expectedAccount = username;
            string expectedStatusCode = statusCode;

            //Act
            Tuple<IRecoveryLink, string> results = await recoveryService.CreateRecoveryLinkAsync(account, cancellationTokenSource.Token).ConfigureAwait(false);
            string resultAccount = results.Item1.Username;
            string resultStatusCode = results.Item2;


            //Assert
            Assert.Equal(expectedAccount, resultAccount);
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        [Theory] //UPDATE WITH SQL SCRIPT
        [InlineData("IntegrationRecoveryService5@gmail.com", "user", "200: Server: success")]
        [InlineData("IntegrationRecoveryService6@gmail.com", "admdin", "404: Database: The recovery link was not found")]
        public async Task RemoveALinkAsync(string username, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = new RecoveryService(SqlDAO, LogService, MessageBank);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            IRecoveryLink toRemove = new RecoveryLink(username, Guid.NewGuid(), DateTime.Now, authorizationLevel);
            await SqlDAO.CreateRecoveryLinkAsync(toRemove).ConfigureAwait(false);
            string expected = statusCode;

            //Act
            string result = await recoveryService.RemoveRecoveryLinkAsync(toRemove, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "IntegrationRecoveryService7@gmail.com", "user", "", "" )]
        [InlineData("", "IntegrationRecoveryService8@gmail.com", "user", "", "")]
        public async Task GetRecoveryLinkAsync(string guid, string username, string authorizationLevel, string timeCreated, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = new RecoveryService(SqlDAO, LogService, MessageBank);
            string expectedUsername = username;
            string expectedGuid = guid;
            string expectedAuthorizationLevel = authorizationLevel;
            string expectedStatusCode = statusCode;
            DateTime expectedTimeCreated = DateTime.Parse(timeCreated);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IRecoveryLink, string> results = await recoveryService.GetRecoveryLinkAsync(guid, cancellationTokenSource.Token).ConfigureAwait(false);
            string resultUsername = results.Item1.Username;
            string resultGuid = results.Item1.GUIDLink.ToString();
            string resultAuthorizationLevel = results.Item1.AuthorizationLevel;
            DateTime resultTimeCreated = results.Item1.TimeCreated;
            string resultStatusCode = results.Item2;

            //Assert
            Assert.Equal(expectedUsername, resultUsername);
            Assert.Equal(expectedGuid, resultGuid);
            Assert.Equal(expectedAuthorizationLevel, resultAuthorizationLevel);
            Assert.Equal(expectedTimeCreated, resultTimeCreated);
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        
    }


}
