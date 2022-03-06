using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;


namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryRegistrationManagerShould
    {

        ISqlDAO _sqlDAO { get; set; }
        ILogService _logService { get; set; }

        IRegistrationService _registrationService { get; set; }

        IMailService _mailService { get; set; }

        IValidationService _validationService { get; set; }

        IMessageBank _messageBank { get; set; }

        IRegistrationManager _registrationManager { get; set; }
        public InMemoryRegistrationManagerShould()
        {
            _sqlDAO = new InMemorySqlDAO();
            _logService = new InMemoryLogService(_sqlDAO);
            _registrationService = new RegistrationService(_sqlDAO, _logService);
            
            _messageBank = new MessageBank();
            _mailService = new MailService(_messageBank);
            _validationService = new ValidationService(_messageBank);
            _registrationManager = new RegistrationManager(_sqlDAO, _logService, _registrationService, _mailService, _validationService, _messageBank);
        }


        [Theory]
        [InlineData("28HoursAgo@gmail.com", -1)]
        [InlineData("12HoursAgo@gmail.com", 0)]
        [InlineData("2DaysAgo@gmail.com", -2)]
        public void CheckConfirmationLinkValidity(string username, int daySubtraction)
        {
            //Act
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + daySubtraction, DateTime.Now.Hour, DateTime.Now.Minute-2, 0);
            IConfirmationLink link = new ConfirmationLink(username, Guid.NewGuid(), time);
            _sqlDAO.CreateConfirmationLink(link);
            bool expected;

            int hours = (-daySubtraction * 24);
            if (hours > 24)
                expected = false;
            else
                expected = true;

            //Act
            bool result = _registrationManager.IsConfirmationLinkValid(link);

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

            //Act
            List<string> result = _registrationManager.CreatePreConfirmedAccount(email, passphrase);

            //Assert
            Assert.Equal('S', result.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.tresearch.systems")]
        [InlineData("pammmmyyyy@gmail.com", "www.tresearch.systems")]
        public void SendEmailConfirmation(string email, string baseUrl)
        {

            IAccount account = new Account(email, "temporaryPassword");


            //Act
            List<string> result = _registrationManager.SendConfirmation(email, baseUrl);

            //Assert
            Assert.Equal('S', result.Last()[0]);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "NowStreaming", "www.tresearch.systems/Account/Verify?t=", "3/5/2022 7:52:04 PM")]
        [InlineData("pammmmyyyy@gmail.com", "HunterGather", "www.tresearch.systems/Account/Verify?t=", "3/5/2022 5:52:04 PM")]
        public void ConfirmAccount(string email, string passphrase, string baseUrl, string date)
        {
            IConfirmationLink _confirmationLink = new ConfirmationLink(email, Guid.NewGuid(), DateTime.Parse(date));

            _sqlDAO.CreateConfirmationLink(_confirmationLink);

            string link = baseUrl + _confirmationLink.UniqueIdentifier;

            List<string> results = new List<string>();

            //Act
            results.AddRange(_registrationManager.ConfirmAccount(link));

            //Assert
            Assert.Equal('S', results.Last()[0]);
        }
    }
}
