using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;


namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IAccountManager
    {
        public IMailService MailService { get; set; }
        public IAccountService AccountService { get; set; }

        public string SendConfirmation(IAccount account, string baseUrl);

        public string CreatePreConfirmedAccount(string email, string passphrase);
    }
}
