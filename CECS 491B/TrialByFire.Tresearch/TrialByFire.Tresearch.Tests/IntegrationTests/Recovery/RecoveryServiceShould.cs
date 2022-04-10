using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class RecoveryServiceShould : TestBaseClass
    {
        public RecoveryServiceShould() : base(new string[] { })
        {
            
            TestServices.AddScoped<IRecoveryService, RecoveryService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("IntegrationRecoveryService1@gmail.com", "200: Server: success", "user")]                
        [InlineData("IntegrationRecoveryService2@gmail.com", "200: Server: success", "admin")]
        [InlineData("IntegrationRecoveryService99@gmail.com", "404: Database: The account was not found.", "user")]
        public async Task GetAccountAsync(string username, string statusCode, string authorizationLevel)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expectedStatusCode = statusCode;
            string expectedUsername = username;


            //Act
            Tuple<IAccount, string> results = await recoveryService.GetAccountAsync(username, authorizationLevel, cancellationTokenSource.Token);
            string resultStatusCode = results.Item2;
            
            //Assert
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        

        [Theory]   
        [InlineData("IntegrationRecoveryService3@gmail.com", "myPassphrase", "409: Database: The recovery link arealdy exists.", "user", false, true)]          //Account already has a link
        [InlineData("IntegrationRecoveryService4@gmail.com", "myPassphrase", "200: Server: success", "user", false, true)]                                      //Account does not have a link
        [InlineData("IntegrationRecoveryService99@gmail.com", "myAccountDoesntExist", "404: Database: The account was not found.", "admin", false, false)]      //Account doesn't exist
        public async Task CreateALinkAsync(string username, string passphrase, string statusCode, string authorizationLevel, bool accountStatus, bool confirmed)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            IAccount account = new Account(username, passphrase, authorizationLevel, accountStatus, confirmed);
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
        [InlineData("IntegrationRecoveryService5@gmail.com", "user", "200: Server: success", true)]     //Account has recovery link
        [InlineData("IntegrationRecoveryService6@gmail.com", "admin", "200: Server: success", false)]    //Account has no recoveryLink
        public async Task RemoveALinkAsync(string username, string authorizationLevel, string statusCode, bool create)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            IRecoveryLink toRemove = new RecoveryLink(username, authorizationLevel, DateTime.Now, Guid.NewGuid());
            if(create)
                await sqlDAO.CreateRecoveryLinkAsync(toRemove);
            string expected = statusCode;

            //Act
            string result = await recoveryService.RemoveRecoveryLinkAsync(toRemove, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService7@gmail.com", "user", "200: Server: success", true)]                            //Recovery link doesn't exist
        [InlineData("IntegrationRecoveryService8@gmail.com", "user", "404: Database: The recovery link was not found", false)]   //Recovery link exists
        public async Task GetRecoveryLinkAsync(string username, string authorizationLevel, string statusCode, bool create)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            Guid toFindGuid = Guid.NewGuid();
            string toFindGuidString = toFindGuid.ToString();
            IRecoveryLink toFind = new RecoveryLink(username, authorizationLevel, DateTime.Now, toFindGuid);
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            if(create)
                await sqlDAO.CreateRecoveryLinkAsync(toFind);
            string expectedStatusCode = statusCode;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IRecoveryLink, string> results = await recoveryService.GetRecoveryLinkAsync(toFindGuidString, cancellationTokenSource.Token).ConfigureAwait(false);
            string resultStatusCode = results.Item2;

            //Assert
            Assert.Equal(expectedStatusCode, resultStatusCode);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService9@gmail.com", "user", "200: Server: success")]                               //Already has 2 links
        [InlineData("IntegrationRecoveryService10@gmail.com", "user", "200: Server: success")]                              // Has no count, procedure will add
        [InlineData("IntegrationRecoveryService99@Gmail.com", "admin", "404: Database: The account was not found.")]        //Account doesn't exist
        public async Task IncrementTotalLinksAsync(string username, string  authorizationLevel, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryService.IncrementRecoveryLinkCountAsync(username, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService11@gmail.com", "user", "200: Server: success")]         //Already has 2 links
        [InlineData("IntegrationRecoveryService12@gmail.com", "admin", "200: Server: success")]        // Has no count, procedure will do nothing
        [InlineData("IntegrationRecoveryService99@Gmail.com", "admin", "200: Server: success")]        //Account doesn't exist, do nothing
        public async Task DecrementTotalLinksAsync(string username, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryService.DecrementRecoveryLinkCountAsync(username, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService13@gmail.com", "user", 2)]         //Already has 2 links
        [InlineData("IntegrationRecoveryService14@gmail.com", "admin", 0)]        // Has no count, procedure will do nothing
        [InlineData("IntegrationRecoveryService99@Gmail.com", "admin", 0)]        //Account doesn't exist, do nothing
        public async Task GetTotalLinksAsync(string username, string authorizationLevel, int count)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            int expected = count;

            //Act
            int result = await recoveryService.GetRecoveryLinkCountAsync(username, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService15@gmail.com", "user", "200: Server: success")]          //Account is disabled
        [InlineData("IntegrationRecoveryService16@gmail.com", "admin", "200: Server: success")]         //Account is enabled
        [InlineData("IntegrationRecoveryService99@Gmail.com", "admin", "404: Database: The account was not found.")]                              //Account doesn't exist
        public async Task EnableAccountAsync(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryService.EnableAccountAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("IntegrationRecoveryService17@gmail.com", "user", "200: Server: success")]          //Account is disabled
        [InlineData("IntegrationRecoveryService18@gmail.com", "admin", "200: Server: success")]         //Account is enabled
        [InlineData("IntegrationRecoveryService99@Gmail.com", "admin", "404: Database: The account was not found.")]                              //Account doesn't exist
        public async Task DsiableAccountAsync(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRecoveryService recoveryService = TestProvider.GetService<IRecoveryService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryService.DisableAccountAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }


}
