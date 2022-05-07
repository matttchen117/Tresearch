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

            TestServices.AddScoped<IMailService, MailService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+IntRegMailServ@gmail.com", "https://trialbyfiretresearch.azurewebsites.net/", "200: Server: success")]
        public async Task SendEmail(string email, string url, string expected)
        {
            //Arrange
            IMailService mailService = TestProvider.GetService<IMailService>();
            
            //Act
            string result = await mailService.SendConfirmationAsync(email, url).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

    }
}
