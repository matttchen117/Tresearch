using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class RecoveryManagerShould: TestBaseClass
    {
        public RecoveryManagerShould() : base(new string[]{ }) 
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<IRecoveryService, RecoveryService>();
            TestServices.AddScoped<IRecoveryManager, RecoveryManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+IntegrationRecoveryManager1@gmail.com", "user", "https://trialbyfiretresearch.azurewebsites.net/Recover/Enable?guid=", "200: Server: success")]
        public async Task SendRecoveryEmailAsync(string username,string authorizationLevel, string baseUrl, string statusCode)
        {
            //Arrange
            IRecoveryManager recoveryManager = TestProvider.GetService<IRecoveryManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryManager.SendRecoveryEmailAsync(username, baseUrl, authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("pammypoor+IntegrationRecoveryManager6@gmail.com", "user", "https://trialbyfiretresearch.azurewebsites.net/Recover/Enable?guid=8ee31b44-c823-4256-a613-cfb1611f0b79", "200: Server: success")]
        public async Task EnableAccountAsync(string username, string authorizationLevel, string url, string statusCode)
        {
            //Arrange
            IRecoveryManager recoveryManager = TestProvider.GetService<IRecoveryManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            string expected = statusCode;

            //Act
            string result = await recoveryManager.EnableAccountAsync(url, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
