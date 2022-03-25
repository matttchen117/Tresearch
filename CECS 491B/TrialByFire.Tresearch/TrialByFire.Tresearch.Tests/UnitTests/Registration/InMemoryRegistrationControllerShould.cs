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
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<ISqlDAO, SqlDAO>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestBuilder.Services.AddScoped<IRegistrationController, RegistrationController>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "myValidPassphrase")]
        [InlineData("pammypoor@gmail.com", "ApplePie!")]
        public async Task RegisterTheUser(string email, string passphrase)
        {
            //Arrange
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();
            
            //Act
            IActionResult results = await registrationController.RegisterAccountAsync(email, passphrase).ConfigureAwait(false);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Theory]
        [InlineData("", "statusCode")]
        [InlineData("", "")]
        public async Task confirmAccount(string guid, string statusCode)
        {
            //Arrange
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();
  
            //Act
            IActionResult results = await registrationController.ConfirmAccountAsync(guid);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(statusCode, objectResult.Value);
        }
    }
}
