using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class MailServiceShould : TestBaseClass
    {
        public MailServiceShould() : base(new string[] { }) 
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+IntRecMailServ@gmail.com", "200: Server: success", "www.google.com")]
        public async Task SendRecoveryEmail(string email, string statusCode, string url)
        {
            //Arrange
            IMailService mailService = TestProvider.GetService<IMailService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await mailService.SendRecoveryAsync(email, url, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(statusCode, result);
        }

    }
}
