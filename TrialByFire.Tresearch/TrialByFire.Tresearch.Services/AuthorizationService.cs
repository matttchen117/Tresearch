using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;

namespace TrialByFire.Tresearch.Services
{
    public class AuthorizationService
    {
        MSSQLDAO mssqlDAO;
        LogService logService;
        public AuthorizationService(MSSQLDAO mssqlDAO, LogService logService)
        {
            this.mssqlDAO = mssqlDAO;
            this.logService = logService;
        }

        public bool GetAccountAuthLevel(Account account, String requiredAuthorizationLevel)
        {
            string authorizationLevel = account.AuthorizationLevel;
            return authorizationLevel == requiredAuthorizationLevel;
        }
    }
}