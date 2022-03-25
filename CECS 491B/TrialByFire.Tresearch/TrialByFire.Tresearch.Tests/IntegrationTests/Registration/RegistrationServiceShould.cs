using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationServiceShould : TestBaseClass
    {
        public RegistrationServiceShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestApp = TestBuilder.Build();
        }


        [Theory]
        [InlineData("wonderbread@gmail.com", "myRegisterPassword", "user", "")]
        [InlineData("orowheat@hotmail.com", "unFortunateName", "user", "")]
        public async Task RegisterTheUserAsync(string email, string passphrase, string authorizationLevel, string statusCode)
        {

            //Arrange 
            IAccount account = new Account(email, email, passphrase, "user", true, false);
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            string expected = statusCode;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string results = await registrationService.CreateAccountAsync(email, passphrase, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, results);
        }


        [Theory]
        [InlineData("wheatIsGreat@gmail.com", "wheatIsGreat@gmail.com", "")]
        [InlineData("whitebread@hotmail.com", "whitebread@hotmail.com", "")]
        public async Task CreateTheLinkAsync(string email, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<IConfirmationLink, string> results = await registrationService.CreateConfirmationAsync(email, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, results.Item2);        // GUID contains 36 characters
        }

        [Theory]
        [InlineData("confirmMe@gmail.com", "user", "")]
        [InlineData("confirmMe2@gmail.com", "user", "")]
        public async Task ConfirmTheUserAsync(string email, string authenticationLevel, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationService.ConfirmAccountAsync(email, authenticationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GetConfirmationLink(string guid, string statusCode)
        {
            //Arrange
            IRegistrationService registrationService = TestApp.Services.GetService<IRegistrationService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            Tuple<IConfirmationLink, string> link = await registrationService.GetConfirmationLinkAsync(guid, cancellationTokenSource.Token);
            string result = link.Item2;

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
