using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;

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

        public bool EnableAccount(string username, string email)
        {
            bool isEnabled = false;
            try
            {
                isEnabled = mssqlDAO.EnableAccount(username, email);
               
            }
            catch(Exception e)
            {
                isEnabled = false;
            }
            return isEnabled;
        }
    }
}