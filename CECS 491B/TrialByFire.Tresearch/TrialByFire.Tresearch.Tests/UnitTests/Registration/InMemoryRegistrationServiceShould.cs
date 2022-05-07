using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationServiceShould : TestBaseClass
    {
        public InMemoryRegistrationServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+UnitSerReg1@gmail.com", "myPassphrase", "user", "200: Server: success")]
        [InlineData("pammypoor+UnitSerReg2@gmail.com", "myPassphrase", "user", "409: Server: UserAccount  already exists")]
        public async Task CreateTheUser(string email,  string passphrase, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            Tuple<int, string> results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results.Item2);
        }

        [Theory]
        [InlineData("pammypoor+UnitSerReg3@gmail.com", "user", "200: Server: success")] //UserAccount exists and is not confirmed
        [InlineData("pammypoor+UnitSerReg4@gmail.com", "user", "200: Server: success")]
        [InlineData("pammypoor+UnitSerReg99@gmail.com", "user", "500: Database: The UserAccount was not found.")]
        public async Task ConfirmTheAccount(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationService.ConfirmAccountAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("pammypoor+UnitSerReg2@gmail.com", "user", "200: Server: success")]
        [InlineData("pammypoor+UnitSerReg3@gmail.com", "user", "409: Database: The confirmation link already exists.")]
        [InlineData("pammypoor+UnitSerReg99@gmail.com", "user", "500: Database: The UserAccount was not found.")]
        public async Task CreateTheLink(string email,  string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestProvider.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            Tuple<IConfirmationLink, string> results = await registrationService.CreateConfirmationAsync(email, authorizationLevel);
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
