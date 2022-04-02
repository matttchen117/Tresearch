
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class MailServiceShould : TestBaseClass
    {
        public MailServiceShould() :base(new string[] { })
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com", "200: Server: success")]
        public async Task SendEmail(string email, string url, string statusCode)
        {
            //Arrange
            IMailService mailService = TestProvider.GetService<IMailService>();
            string expected = statusCode;
            //Act
            string result = await mailService.SendConfirmationAsync(email, url).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

    }
}
