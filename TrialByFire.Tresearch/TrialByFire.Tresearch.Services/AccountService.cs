using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.UserManagement
{
    public class AccountService
    {
        MSSQLDAO mssqlDAO;
        LogService logService;

        public AccountService(MSSQLDAO mssqlDAO, LogService logService)
        {
            this.mssqlDAO = mssqlDAO;
            this.logService = logService;
        }

        public bool CreateAccount(string email, string passphrase, string authorizationLevel)
        {
            Account account = new Account(string email, string passphrase, string authorizationLevel);
            return true;
        }
    }
}
