using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;

namespace TrialByFire.Tresearch.Services
{
    public class AuthenticationService
    {
        private MSSQLDAO mssqlDAO;
        private LogService logService;
        private Account userAccount;

        public AuthenticationService(MSSQLDAO mssqlDAO, LogService logService)
        {
            this.mssqlDAO = mssqlDAO;
            this.logService = logService;
        }

        public Account GetAccount(String username, string password)
        {
            userAccount = mssqlDAO.GetAccount(username, password);
            return userAccount;
        }
    }
}
