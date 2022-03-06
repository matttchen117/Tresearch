using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Registration
{
    public class InMemoryMailServiceShould
    {
        public IMailService _mailService;

        public IMessageBank _messageBank;

        public InMemoryMailServiceShould()
        {
            
            _messageBank = new MessageBank();
            _mailService = new MailService(_messageBank);
        }

        [Theory]
        [InlineData("pammypoor@gmail.com", "www.google.com")]
        [InlineData("pammmmyyyy@gmail.com", "https://github.com/Drakat7/Tresearch")]
        public void SendEmail(string email, string url)
        {
            

            //Act
            string result = _mailService.SendConfirmation(email, url);

            //Assert
            Assert.Equal("Success - Confirmation email sent", result);
        }

    }
}
