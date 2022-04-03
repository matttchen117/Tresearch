using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationManagerShould : TestBaseClass
    {


        public InMemoryRegistrationManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<IRegistrationManager, RegistrationManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+UnitManReg1@gmail.com", "undergroundBasketWeaving", "user", "200: Server: success")]
        [InlineData("pammypoor+UnitManReg2@gmail.com", "myPassphrase", "user", "409: Server: Account  already exists")]
        public async Task CreateTheUserAccount(string email, string passphrase, string authorizationLevel, string statusCode)
        {
            //Arrange
            IRegistrationManager registrationManager = TestProvider.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm?guid=";

            //Act
            string result = await registrationManager.CreateAndSendConfirmationAsync(email, passphrase, authorizationLevel, baseUrl, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(statusCode, result);
        }

        public async Task ConfirmAccount(string guid, string statusCode)
        {
            string[] splitExpectation;
            splitExpectation = statusCode.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2]);
            IRegistrationManager registrationManager = TestProvider.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationManager.ConfirmAccountAsync(guid, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
