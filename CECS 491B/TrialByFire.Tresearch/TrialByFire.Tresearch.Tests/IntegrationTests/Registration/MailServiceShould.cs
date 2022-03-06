
using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class MailServiceShould
    {
        IMessageBank _messageBank { get; set; }

        IMailService _mailService { get; set; }
        public MailServiceShould()
        {
            _messageBank = new MessageBank();
            _mailService = new MailService(_messageBank);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com")]
        public void SendEmail(string email, string url)
        {
            //Act
            string result = _mailService.SendConfirmation(email, url);

            //Assert
            Assert.Equal("Success - Confirmation email sent", result);
        }

    }
}
