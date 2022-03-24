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
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();
            IAccount account = new Account(email, email, passphrase, "User", true, false);
            //Act
            IActionResult results = registrationController.RegisterAccount(email, passphrase);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Theory]
        [InlineData("pammypoor+INcontrollerSendConfirmation1@gmail.com")]
        [InlineData("pammypoor+INcontrollerSendConfirmation2@gmail.com")]
        public void SendConfirmation(string email)
        {
            //Arrange
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();
            IAccount account = new Account(email, "temporaryPassword");

            //Act
            IActionResult results = registrationController.SendConfirmation(email);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }


        [Theory]
        [InlineData("pammypoor+INcontrollerConfirm1@gmail.com", "myControllerPass", "www.tresearch.systems/Registration/verify?=", "2022-03-06 21:32:59.910")]
        [InlineData("pammypoor+INcontrollerConfirm2@gmail.com", "myControllerPassword", "www.tresearch.systems/Registration/verify?=", "2022-03-06 21:32:59.910")]
        public async Task confirmAccount(string email, string passphrase, string url, string date)
        {
            //Arrange
            IAccount _account = new Account(email, email, passphrase, "User", true, false);
            IConfirmationLink _confirmationLink = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Parse(date));
            IRegistrationController registrationController = TestApp.Services.GetService<IRegistrationController>();
            ISqlDAO sqlDAO = TestApp.Services.GetService<ISqlDAO>();
            sqlDAO.CreateAccount(_account);
            sqlDAO.CreateConfirmationLink(_confirmationLink);

            //Act
            IActionResult results = registrationController.ConfirmAccount(url + _confirmationLink.UniqueIdentifier);
            var objectResult = results as ObjectResult;

            //Assert
            Assert.Equal(200, objectResult.StatusCode);
        }
    }
}
