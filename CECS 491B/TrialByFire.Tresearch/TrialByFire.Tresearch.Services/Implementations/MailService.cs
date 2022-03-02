using SendGrid;
using SendGrid.Helpers.Mail;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
   public  class MailService: IMailService 
    {
        private string APIKey = "";
        private string sender = "no-reply@tresearch.systems";
        private string senderName = "Tresearch Support";
        private string confirmationTemplate = "";
        private string OTPTemplate = "";

        public MailService() { }

        public string SendConfirmation(string email, string url)
        {
            try
            {
                var client = new SendGridClient(APIKey);
                var confirmation = new SendGridMessage();
                confirmation.SetFrom(sender, senderName);
                confirmation.AddTo(email);
                confirmation.SetTemplateId(confirmationTemplate);
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
    }
}
