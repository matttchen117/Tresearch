using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationServiceShould : TestBaseClass
    {
        public InMemoryRegistrationServiceShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestApp = TestBuilder.Build();
        }


        [Theory]
        [InlineData("", "user", "")]
        [InlineData("", "user", "")]
        public async Task ConfirmTheAccount(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationService.ConfirmAccountAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "", "", "user", "")]
        [InlineData("", "", "", "user", "")]
        public async Task CreateTheUser(string email, string username, string passphrase, string authorizationLevel, string statusCode)
        {
            //Arrange
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results);
        }


        [Theory]
        [InlineData("", "", "")]
        [InlineData("", "", "")]
        public async Task CreateTheLink(string email,  string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
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
