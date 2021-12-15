using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using TrialByFire.Tresearch.Logging;

namespace TrialByFire.Tresearch.Services
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
            bool createAccountSuccessful = false;
            Account account = new Account(email, passphrase, authorizationLevel);
            try
            {
                createAccountSuccessful = mssqlDAO.CreateAccount(account);
            }
            catch (Exception e)
            {
                createAccountSuccessful = false;
            }
            return createAccountSuccessful;
        }
        
        public bool UpdateAccount(string username, string newPassphrase, string newEmail, string newAuthorization)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = mssqlDAO.UpdateAccount(username, newPassphrase, newEmail, newAuthorization);
            }
            catch(Exception e)
            {
                isUpdated = false;
            }
            return isUpdated;
        }

        public bool DeleteAccount(string username)
        {
            bool isDeleted = false;
            try
            {
                isDeleted = mssqlDAO.DeleteAccount(username);
            }
            catch (Exception e)
            {
                isDeleted = false;
            }
            return isDeleted;
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

        public bool DisableAccount(string username)
        {
            bool isDisabled = false;
            try
            {
                isDisabled = mssqlDAO.DisableAccount(username);

            }
            catch (Exception e)
            {
                isDisabled = false;
            }
            return isDisabled;
        }
    }
}
