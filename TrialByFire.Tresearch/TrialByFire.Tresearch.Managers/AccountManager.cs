using System;
using TrialByFire.Tresearch.DAO;

namespace TrialByFire.Tresearch.UserManagement
{
    public class AccountManager
    {
        private MSSQLDAO mssqlDAO;
        private LogService logService;
        private Account userAccount;

        public AccountManager(MSSQLDAO mssqlDAO, LogService logService)
        {
            this.mssqlDAO = mssqlDAO;
            this.logService = logService;
        }

        public bool CreateAccount(string email, string passphrase, string authorizationLevel)
        {
        }
    }
}