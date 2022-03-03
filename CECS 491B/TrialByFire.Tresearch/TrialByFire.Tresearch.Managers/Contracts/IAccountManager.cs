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
        public IMailService _mailService { get; set; }
        public IAccountService _accountService { get; set; }

        public string SendConfirmation(IAccount account, string baseUrl);

        public string CreatePreConfirmedAccount(string email, string passphrase);
    }
}
