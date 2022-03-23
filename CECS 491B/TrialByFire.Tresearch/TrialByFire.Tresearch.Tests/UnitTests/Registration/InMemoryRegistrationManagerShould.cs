using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationManagerShould : TestBaseClass
    {

        
        public InMemoryRegistrationManagerShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IMailService, MailService>();
            TestBuilder.Services.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            TestBuilder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
            TestApp = TestBuilder.Build();
        }


        [Theory]
        [InlineData("28HoursAgo@gmail.com", -1)]
        [InlineData("no@gmail.com", 0)]
        [InlineData("2DaysAgo@gmail.com", -2)]
        public void CheckConfirmationLinkValidity(string username, int daySubtraction)
        {
            //Act
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + daySubtraction, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            IConfirmationLink link = new ConfirmationLink(username, Guid.NewGuid(), time);
            ISqlDAO sqlDAO = TestApp.Services.GetService<ISqlDAO>();
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();

            sqlDAO.CreateConfirmationLink(link);
            bool expected;

            int hours = (-daySubtraction * 24);
            if (hours >= 24)
                expected = false;
            else
                expected = true;

            //Act
            bool result = registrationManager.IsConfirmationLinkValid(link);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("wonderbread@gmail.com", "travelPlans123")]
        [InlineData("catcherInTheRye@hotmail.com", "undergroundBasketWeaving")]
        [InlineData("windows365@gmail.com", "myPassphrase123")]
        public void CreateTheUserAccount(string email, string passphrase)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            //Act
            List<string> result = registrationManager.CreatePreConfirmedAccount(email, passphrase);

            //Assert
            Assert.Equal('S', result.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.tresearch.systems")]
        [InlineData("pammmmyyyy@gmail.com", "www.tresearch.systems")]
        public void SendEmailConfirmation(string email, string baseUrl)
        {
            //Arrange
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            IAccount account = new Account(email, "temporaryPassword");


            //Act
            List<string> result = registrationManager.SendConfirmation(email, baseUrl);

            //Assert
            Assert.Equal('S', result.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "NowStreaming", "www.tresearch.systems/Account/Verify?t=", "3/7/2022 7:52:04")]
        [InlineData("pammmmyyyy@gmail.com", "HunterGather", "www.tresearch.systems/Account/Verify?t=", "3/7/2022 5:52:04")]
        public void ConfirmAccount(string email, string passphrase, string baseUrl, string date)
        {
            IConfirmationLink _confirmationLink = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Parse(date));
            IAccount _account = new Account(email, email, passphrase, "user", true, false);
            ISqlDAO sqlDAO = TestApp.Services.GetService<ISqlDAO>();
            IRegistrationManager registrationManager = TestApp.Services.GetService<IRegistrationManager>();
            sqlDAO.CreateAccount(_account);
            sqlDAO.CreateConfirmationLink(_confirmationLink);

            string link = baseUrl + _confirmationLink.UniqueIdentifier;

            List<string> results = new List<string>();

            //Act
            results.AddRange(registrationManager.ConfirmAccount(link));

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
    }
}
