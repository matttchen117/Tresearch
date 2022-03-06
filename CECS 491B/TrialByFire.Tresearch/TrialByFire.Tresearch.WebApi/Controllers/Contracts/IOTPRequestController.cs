namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IOTPRequestController
    {
        public string RequestOTP(string username, string passphrase, string authorizationLevel);
    }
}
