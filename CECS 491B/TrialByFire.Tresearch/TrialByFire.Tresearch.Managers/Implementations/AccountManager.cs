using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class AccountManager
    {
        private IMailService _mailService { get; set; }
        private IAccountService _accountService { get; set; }

        public string SendConfirmation(IAccount account, string baseUrl)
        {
            try
            {
                string linkUrl = _accountService.CreateConfirmation(account, baseUrl);
                _mailService.SendConfirmation(account.email, linkUrl);
            } catch
            {
                return "Failed - Unable to send confirmation link";
            }
            return "Success - Confirmation Sent";
        }
    }
}
