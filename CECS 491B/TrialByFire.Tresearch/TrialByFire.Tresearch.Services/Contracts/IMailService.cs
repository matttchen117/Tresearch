
namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IMailService
    {
        public string SendConfirmation(string email, string url);
    }
}
