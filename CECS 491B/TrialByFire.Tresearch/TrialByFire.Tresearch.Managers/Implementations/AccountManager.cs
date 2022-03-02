using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class AccountManager: IAccountManager
    {
        public IMailService _mailService { get; set; }
        public IAccountService _accountService { get; set; }

        private string defaultAuthorization = "user";

        public string CreatePreConfirmedAccount(string email, string passphrase)
        {
            string code;
            try
            {
                Account _account = new Account(email, passphrase, defaultAuthorization, true, false);
                code = _accountService.CreatePreConfirmedAccount(_account);

            } catch
            {
                return "Failed - Unable to Create Account";
            }
            return code;
        }

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
