using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IRegistrationController
    {
        public string RegisterAccount(string email, string passphrase);

        public string SendConfirmation(string email);

        public string ConfirmAccount(string url);
    }
}
