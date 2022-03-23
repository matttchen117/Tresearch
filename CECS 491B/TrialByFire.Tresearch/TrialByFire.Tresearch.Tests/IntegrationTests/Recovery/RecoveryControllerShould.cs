using Xunit;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class RecoveryControllerShould: TestBaseClass
    {
        public RecoveryControllerShould() : base(new string[] { }) 
        {
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<IRecoveryService, RecoveryService>();
            TestBuilder.Services.AddScoped<IRecoveryManager, RecoveryManager>();
            TestBuilder.Services.AddScoped<IRecoveryController, RecoveryController>();
            TestApp = TestBuilder.Build();
        }
        [Theory]
        [InlineData("pammypoor+IntRecoveryController1@gmail.com", "user", "200: Server: success")]
        public async Task SendRecoveryEmail(string email, string authorizationlevel, string statusCode)
        {
            //Arrange
            string[] splitExpectation;
            splitExpectation = statusCode.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]) };
            IRecoveryController recoveryController = TestApp.Services.GetService<IRecoveryController>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            IActionResult result = await recoveryController.SendRecoveryEmailAsync(email, authorizationlevel).ConfigureAwait(false);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
