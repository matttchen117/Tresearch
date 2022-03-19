
namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IMailService
    {
        public string SendConfirmation(string email, string url);

        public string SendOTP(string email, string subject, string plainBody, string htmlBody);

        public Task<string> SendRecoveryAsync(string email, string url, CancellationToken cancellationToken = default(CancellationToken));

    }
}
