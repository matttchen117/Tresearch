using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountService : IAccountService
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }

        private int linkActivationLimit = 24;
        public string CreatePreConfirmedAccount(IAccount account)
        {
            try
            {
                bool isAccountCreated = _sqlDAO.CreateAccount(account);
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
            Guid activationGuid;
            try
            {
                activationGuid = Guid.NewGuid();
                var linkUrl = $"{baseUrl}/Account/Verify?t={activationGuid}";
                IConfirmationLink _confirmationLink= new ConfirmationLink(account.Username, activationGuid, DateTime.Now);
                bool isAccountCreated = _sqlDAO.CreateConfirmationLink(_confirmationLink);
                if (!isAccountCreated)
                    return null;
            }
            catch
            {
                return null;
            }
            return activationGuid.ToString();
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

        public IConfirmationLink GetConfirmationLinkInfo(string url)
        {
            IConfirmationLink _confirmationLink = _sqlDAO.GetConfirmationLink(url);
            return _confirmationLink;

        }

        public bool isConfirmationLinkValid(IConfirmationLink confirmationLink)
        {
            DateTime now = DateTime.Now;
            if (confirmationLink.timestamp > now.AddHours(-linkActivationLimit) && confirmationLink.timestamp <= now)
                return true;
            else
                return false;
        }

        public string removeConfirmationLink(IConfirmationLink confirmationLink)
        {

            return "Success - Confirmation link removed";
        }

        public string CreatePreRegisteredAccount(IAccount account)
        {
            throw new NotImplementedException();
        }
    }
}
