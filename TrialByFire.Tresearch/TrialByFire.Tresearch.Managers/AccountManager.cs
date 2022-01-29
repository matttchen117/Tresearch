using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;

namespace TrialByFire.Tresearch.Managers
{
    public class AccountManager
    {
        private SqlDAO mssqlDAO;
        private LogService logService;
        private Account userAccount;

        public AccountManager(SqlDAO mssqlDAO, LogService logService)
        {
            this.mssqlDAO = mssqlDAO;
            this.logService = logService;
        }

       public bool CreateAccount(string email, string passphrase, string authorizationLevel)
        {
            bool createAccountSuccessful = false;
            AccountService accountService = new AccountService(mssqlDAO, logService);
            try
            {
                createAccountSuccessful = accountService.CreateAccount(email, passphrase, authorizationLevel);
            }
            catch (Exception e)
            {
                logService.CreateLog(DateTime.Now, "Error", userAccount.Username, "Business", "An error occurred when trying to create the account.");
                createAccountSuccessful = false;
            }
            return createAccountSuccessful;
        }

        public bool DeleteAccount(string username)
        {
            bool isDeleted = false;
            AccountService accountService = new AccountService(mssqlDAO, logService);
            try
            {
                isDeleted = accountService.DeleteAccount(username);
            }
            catch(Exception e)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        public bool UpdateAccount(string username, string newPassphrase, string newEmail, string newAuthorizationLevel)
        {
            bool isUpdated = false;
            AccountService accountService = new AccountService(mssqlDAO, logService);
            try
            {
                isUpdated = accountService.UpdateAccount(username, newPassphrase, newEmail, newAuthorizationLevel);
            }
            catch (Exception ex)
            {
                isUpdated = false;
            }
            return isUpdated;
        }
        
         public bool EnableAccount(string username, string email)
        {
            AccountService accountService = new AccountService(mssqlDAO, logService);
            bool isEnabled = accountService.EnableAccount(username, email);
            return isEnabled;
        }

        public bool DisableAccount(string username)
        {
            AccountService accountService = new AccountService(mssqlDAO, logService);
            bool isEnabled = accountService.DisableAccount(username);
            return isEnabled;
        }
    }
}
