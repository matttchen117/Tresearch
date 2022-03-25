using SendGrid;
using SendGrid.Helpers.Mail;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Models;

namespace TrialByFire.Tresearch.Services.Implementations
{
   public  class MailService: IMailService 
    {
        private IMessageBank _messageBank { get; }
        private BuildSettingsOptions _options { get; }

        private string _sender = "no-reply@tresearch.systems";
        private string _senderName = "Tresearch Support";
        private string _confirmationTemplate = "d-a7af897441a34066b64fe416cf76d29b";
        private string _recoveryTemplate = "d-a8d61f53ced342a9addb30a043e6ef01";

        public MailService(IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options) 
        { 
            _messageBank = messageBank;
            _options = options.Value;
        }

        public async Task<string> SendConfirmationAsync(string email, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var client = new SendGridClient(_options.SendGridAPIKey);
                var confirmation = new SendGridMessage();
                confirmation.SetFrom(_sender, _senderName);
                confirmation.AddTo(email);
                confirmation.SetTemplateId(_confirmationTemplate);
                confirmation.SetTemplateData(new
                {
                    url = url
                });
                var result = client.SendEmailAsync(confirmation).Result;
                return _messageBank.GetMessage(IMessageBank.Responses.sendEmailFail).Result;
            } 
            catch(Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<string> SendOTPAsync(string email, string subject, string plainBody, string htmlBody, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var client = new SendGridClient(_options.SendGridAPIKey);
                var from = new EmailAddress(_sender, _senderName);
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainBody, htmlBody);
                var response = await client.SendEmailAsync(msg, cancellationToken).ConfigureAwait(false);
            } catch
            {
                return _messageBank.GetMessage(IMessageBank.Responses.sendEmailFail).Result;
            }
            return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
        }

        public async Task<string> SendRecoveryAsync(string email, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var client = new SendGridClient(_options.SendGridAPIKey);
                var confirmation = new SendGridMessage();
                confirmation.SetFrom(_sender, _senderName);
                confirmation.AddTo(email);
                confirmation.SetTemplateId(_recoveryTemplate);
                confirmation.SetTemplateData(new
                {
                    url = url
                });
                cancellationToken.ThrowIfCancellationRequested();
                var result = client.SendEmailAsync(confirmation).Result;
                return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;

            }
            catch(Exception ex)
            {
                return _messageBank.GetMessage(IMessageBank.Responses.sendEmailFail).Result;
            }
        }
    }
}
