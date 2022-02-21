using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services
{
    public class MailService
    {
        private static string APIKEY = "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";
        private static string sender = "no-reply@tresearch.systems";
        private static string senderName = "Tresearch Support";
        private static string confirmationTemplate = "d-debf34255d114608b1af24a054fe1f42";
        private static string OTPTemplate;

        public void SendEmail(string email, string subject, string plainBody, string htmlBody)
        {
            Execute(email, subject, plainBody, htmlBody).Wait();
        }
        public static async Task Execute(string email, string subject, string plainBody, string htmlBody)
        {
            var client = new SendGridClient(APIKEY);
            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainBody, htmlBody);
            var response = await client.SendEmailAsync(msg);
        }

        public string SendConfirmation(string email, string url)
        {
            try
            {
                var client = new SendGridClient(APIKEY);
                var confirmation = new SendGridMessage();
                confirmation.SetFrom(sender, senderName);
                confirmation.AddTo(email);
                confirmation.SetTemplateId(confirmationTemplate);
                confirmation.SetTemplateData(new
                {
                    url = url
                });
                var result = client.SendEmailAsync(confirmation).Result;
            }
            catch
            {
                return "Failed";
            }

            return "Confirmation Sent";

        }

    }
}
