using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationControllerShould : TestBaseClass
    {
        public InMemoryRegistrationControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IRegistrationService, RegistrationService>();
            TestServices.AddScoped<IRegistrationManager, RegistrationManager>();
            TestServices.AddScoped<IRegistrationController, RegistrationController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("pammypoor+UnitConrReg1@gmail.com", "myValidPassphrase", "200: Server:  success")]     //UserAccount doesn't exist
        [InlineData("pammypoor+UnitConrReg2@gmail.com", "myPassphrase", "409: Server: UserAccount  already exists")]
        public async Task RegisterTheUser(string email, string passphrase, string statusCode)
        {
            //Arrange
            string[] splitExpectation;
            splitExpectation = statusCode.Split(":");
            ObjectResult expectedResult = new ObjectResult(splitExpectation[2])
            { StatusCode = Convert.ToInt32(splitExpectation[0]),
              Value = splitExpectation[2]};
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();
            
            //Act
            IActionResult results = await registrationController.RegisterAccountAsync(email, passphrase).ConfigureAwait(false);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
        }

        public async Task confirmAccount(string guid, string statusCode)
        {
            //Arrange
            IRegistrationController registrationController = TestProvider.GetService<IRegistrationController>();
  
            //Act
            IActionResult results = await registrationController.ConfirmAccountAsync(guid);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(statusCode, objectResult.Value);
        }
    }
}
