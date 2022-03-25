using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryMailServiceShould : TestBaseClass
    {
        public InMemoryMailServiceShould() : base(new string[] { })
        {

            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "https://trialbyfiretresearch.azurewebsites.net/")]
        [InlineData("pammmmyyyy@gmail.com", "https://trialbyfiretresearch.azurewebsites.net/")]
        public async Task SendEmail(string email, string url)
        {
            //Arrange
            IMailService mailService = TestApp.Services.GetService<IMailService>();
            
            //Act
            string result = await mailService.SendConfirmationAsync(email, url).ConfigureAwait(false);

            //Assert
            Assert.Equal("Success - Confirmation email sent", result);
        }

    }
}
