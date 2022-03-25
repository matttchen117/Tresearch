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
        [InlineData("skiPatrol@gmail.com", "myRegisterPassword", "", "")]
        [InlineData("snowboarder@hotmail.com", "unFortunateName", "", "")]
        public async Task RegisterTheUser(string email, string passphrase, string authorizationLevel, string baseUrl)
        {
            //Arrange 
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            //Act
            string results = await registrationManager.CreateAndSendConfirmationAsync(email, passphrase, authorizationLevel, baseUrl).ConfigureAwait(false);

            //Assert
            Assert.Equal("success" , results);
        }

        [Theory]
        [InlineData("confirmMeMan@gmail.com", "", "" )]
        [InlineData("confirmMeMan2@gmail.com", "", "")]
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
        public void checkLinkValidity(int minusHours)
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
            bool actual = registrationManager.IsConfirmationLinkValid(confirmationLink);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
