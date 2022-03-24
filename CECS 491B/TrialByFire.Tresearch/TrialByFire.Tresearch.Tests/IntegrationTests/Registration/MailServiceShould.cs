
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
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com", "200: Server: success")]
        public void SendEmail(string email, string url, string statusCode)
        {
            //Arrange
            IMailService mailService = TestApp.Services.GetService<IMailService>();
            string expected = statusCode;
            //Act
            string result = mailService.SendConfirmation(email, url);

            //Assert
            Assert.Equal(expected, result);
        }

    }
}
