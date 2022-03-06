namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IAuthenticationController
    {
        public string Authenticate(string username, string otp, string role, DateTime now);
    }
}
