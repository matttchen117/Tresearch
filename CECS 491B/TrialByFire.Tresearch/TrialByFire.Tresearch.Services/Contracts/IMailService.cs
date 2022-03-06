
namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IMailService
    {
        public string SendConfirmation(string email, string url);

        public string SendOTP(string email, string subject, string plainBody, string htmlBody);


    }
}
