using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Xunit;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationControllerShould : TestBaseClass
    {
        public RegistrationControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<IRegistrationManager, RegistrationManager>();
            TestServices.AddScoped<IRegistrationController, RegistrationController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("trialbyfire.tresearch+IntRegCon1@gmail.com", "myPassword", "200: Server: success")]
        [InlineData("trialbyfire.tresearch+IntRegCon2@gmail.com", "myPassword", "409: Server: Account  already exists")]
        public async Task RegisterTheUser(string email, string passphrase, string statusCode)
        {
            // Arrange
            string[] splitExpectation;
            splitExpectation = statusCode.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]) };
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();

            //Act
            IActionResult results = await registrationController.RegisterAccountAsync(email, passphrase).ConfigureAwait(false); 
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }

        [Theory]
        [InlineData("f92c1817-a3b0-4bce-b96a-5deb38ccf05d", "200: Server: success")]
        [InlineData("3404629b-af83-4d70-8cd2-bd5cfd65e9ea", "200: Server: success")]
        [InlineData("7d8929f6-40b9-441d-9b48-1ea24e60fcf1", "404: Database: The confirmation link was not found.")]
        public async void ConfirmTheUser(string guid, string statusCode)
        {
            //Arrange
            string[] splitExpectation;
            splitExpectation = statusCode.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]) };
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();

            //Act
            IActionResult results = await registrationController.ConfirmAccountAsync(guid).ConfigureAwait(false);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }

    }
}
