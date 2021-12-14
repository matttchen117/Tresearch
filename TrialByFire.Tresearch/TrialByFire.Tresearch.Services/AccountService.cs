using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;

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

        public bool EnableAccount(string username, string email)
        {
            bool isEnabled = false;
            try
            {
                isEnabled = mssqlDAO.EnableAccount(username, email);

            }
            catch (Exception e)
            {
                isEnabled = false;
            }
            return isEnabled;
        }
    }
}
