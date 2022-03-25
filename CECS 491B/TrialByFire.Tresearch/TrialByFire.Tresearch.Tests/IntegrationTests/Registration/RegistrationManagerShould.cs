using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationManagerShould : TestBaseClass
    {
        public RegistrationManagerShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestApp = TestBuilder.Build();
        }
        [Theory]
        [InlineData("trialbyfire.tresearch+IntRegMan1@gmail.com", "myPassword","user", "200: Server: success")]
        [InlineData("trialbyfire.tresearch+IntRegMan2@gmail.com", "unFortunateName","user", "409: Server: Account  already exists")]
        public async Task RegisterTheUser(string email, string passphrase, string authorizationLevel, string statusCode)
        {
            //Arrange 
            string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm?guid=";
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            //Act
            string results = await registrationManager.CreateAndSendConfirmationAsync(email, passphrase, authorizationLevel, baseUrl).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode , results);
        }

        [Theory]
        [InlineData("trialbyfire.tresearch+IntRegMan4@gmail.com", "7f5c634a-ef48-49c2-a20c-adde83b6837d", "200: Server: success")]
        [InlineData("trialbyfire.tresearch+IntRegMan5@gmail.com", "5278f32c-353f-487d-9145-4320125fc433", "200: Server: success")] //User is already enabled
        [InlineData("trialbyfire.tresearch+IntRegMan5@gmail.com", "7f5c634a-ef47-49c2-a20c-adde85b6837d", "404: Database: The confirmation link was not found.")]
        public async Task ConfirmTheUser(string username, string guid, string statusCode)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await registrationManager.ConfirmAccountAsync(guid, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-24)]
        [InlineData(-23)]
        [InlineData(0)]
        public void checkLinkInValidity(int minusHours)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
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
    }
}
