using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationManagerShould : TestBaseClass
    {


        public InMemoryRegistrationManagerShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("wonderbread@gmail.com", "travelPlans123", "user")]
        [InlineData("catcherInTheRye@hotmail.com", "undergroundBasketWeaving", "user")]
        [InlineData("windows365@gmail.com", "myPassphrase123", "user")]
        public async Task CreateTheUserAccount(string email, string passphrase, string authorizationLevel)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm?guid=";

            //Act
            string result = await registrationManager.CreateAndSendConfirmationAsync(email, passphrase, authorizationLevel, baseUrl, cancellationTokenSource.Token);

            //Assert
            Assert.Equal("success", result);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task ConfirmAccount(string guid, string statusCode)
        {

            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationManager.ConfirmAccountAsync(guid, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
