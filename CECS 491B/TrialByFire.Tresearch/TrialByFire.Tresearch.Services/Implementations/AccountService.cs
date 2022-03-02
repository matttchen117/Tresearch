using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountService : IAccountService
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public string CreatePreConfirmedAccount(IAccount account)
        {
            try
            {
                Boolean isAccountCreated = _sqlDAO.CreateAccount(account);
                if (!isAccountCreated)
                    return "Failed - Cannot add account to database";
            }
            catch
            {
                return "Failed - Account not created";
            }

            return "Success - Account Created";
        }

        public string CreateConfirmation(IAccount account, string baseUrl)
        {
            string activationGuid;
            try
            {
                activationGuid = Guid.NewGuid().ToString();
                var linkUrl = $"{baseUrl}/Account/Verify?t={activationGuid}";
                bool isAccountCreated = _sqlDAO.CreateConfirmationLink(account, baseUrl);
                if (!isAccountCreated)
                    return null;
            }
            catch
            {
                return null;
            }
            return activationGuid;
        }

        public string ConfirmAccount(string email)
        {
            try
            {

            }
            catch
            {

            }
            return "Success - Account Confirmed";
        }

    }
}
