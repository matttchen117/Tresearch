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
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestBuilder.Services.AddScoped<IRegistrationController, RegistrationController>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("fezAshtray@gmail.com", "myRegisterPassword")]
        [InlineData("cassieKat@hotmail.com", "unFortunateName")]
        public async Task RegisterTheUser(string email, string passphrase)
        {
            // Arrange
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();

            //Act
            IActionResult results = await registrationController.RegisterAccount(email, passphrase).ConfigureAwait(false); ;
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }

        public void ConfirmTheUser(string url)
        {

        }

    }
}
