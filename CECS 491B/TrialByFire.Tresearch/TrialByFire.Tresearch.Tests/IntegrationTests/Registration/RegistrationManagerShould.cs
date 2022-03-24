using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class RegistrationManagerShould : TestBaseClass
    {
        public RegistrationManagerShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestApp = TestBuilder.Build();
        }
        [Theory]
        [InlineData("skiPatrol@gmail.com", "myRegisterPassword")]
        [InlineData("snowboarder@hotmail.com", "unFortunateName")]
        public void RegisterTheUser(string email, string passphrase)
        {
            //Arrange 
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            //Act
            List<string> results = registrationManager.CreatePreConfirmedAccount(email, passphrase);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }

        [Theory]
        [InlineData("confirmMeMan@gmail.com", "myPassword", "user", true, false)]
        [InlineData("confirmMeMan2@gmail.com", "myPassword", "user", true, false)]
        public async Task ConfirmTheUser(string email, string passphrase, string authenticationLevel, bool status, bool confirmed)
        {
            //Arrange
            IConfirmationLink link = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Now);
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            ISqlDAO sqlDAO = TestApp.Services.GetService<ISqlDAO>();
            string url = link.UniqueIdentifier.ToString();
            sqlDAO.CreateConfirmationLink(link);
            IAccount _account = new Account(email, email, passphrase, authenticationLevel, status, confirmed);
            await SqlDAO.CreateAccountAsync(_account);

            //Act
            List<string> results = registrationManager.ConfirmAccount(url);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com")]
        [InlineData("jelazo@live.com", "www.tresearch.systems/")]
        public void SendConfirmation(string email, string url)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();

            //Act
            List<string> results = registrationManager.SendConfirmation(email, url);

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }


        [Theory]
        [InlineData("cottonEyedJoe@gmail.com", -1)]
        [InlineData("innnout@live.com", 0)]
        public void checkLinkValidity(string email, int sub)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            DateTime now = DateTime.Now;
            IConfirmationLink link = new ConfirmationLink(email, Guid.NewGuid(), now.AddDays(sub));
            Boolean expected;
            if (sub < 0)
                expected = false;
            else
                expected = true;

            //Act
            bool actual = registrationManager.IsConfirmationLinkValid(link);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
