
namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IMailService
    {
        public Task<string> SendConfirmationAsync(string email, string url, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> SendOTPAsync(string email, string subject, string plainBody, string htmlBody, 
            CancellationToken cancellationToken = default);

        public Task<string> SendRecoveryAsync(string email, string url, CancellationToken cancellationToken = default(CancellationToken));

    }
}
