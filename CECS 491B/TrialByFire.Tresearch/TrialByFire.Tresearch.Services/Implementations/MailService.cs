using SendGrid;
using SendGrid.Helpers.Mail;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
   public  class MailService: IMailService 
    {
        private IMessageBank _messageBank { get; }
        private string _APIKey = "";
        private string _sender = "no-reply@tresearch.systems";
        private string _senderName = "Tresearch Support";
        private string _confirmationTemplate = "d-a7af897441a34066b64fe416cf76d29b";
        private string _OTPTemplate = "";

        public MailService(IMessageBank messageBank) 
        { 
            _messageBank = messageBank;
        }

        public string SendConfirmation(string email, string url)
        {
            try
            {
                var client = new SendGridClient(_APIKey);
                var confirmation = new SendGridMessage();
                confirmation.SetFrom(_sender, _senderName);
                confirmation.AddTo(email);
                confirmation.SetTemplateId(_confirmationTemplate);
                confirmation.SetTemplateData(new
                {
                    url = url
                });
                var result = client.SendEmailAsync(confirmation).Result;
            } catch
            {
                return "Failed - Couldn't send confirmation email";
            }
            return "Success - Confirmation email sent";
        }

        public string SendOTP(string email, string subject, string plainBody, string htmlBody)
        {
            try
            {
                var client = new SendGridClient(_APIKey);
                var from = new EmailAddress(_sender, _senderName);
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainBody, htmlBody);
                var response = client.SendEmailAsync(msg);
            } catch
            {
                return _messageBank.ErrorMessages["sendEmailFail"];
            }
            return _messageBank.SuccessMessages["generic"];
        }
    }
}
