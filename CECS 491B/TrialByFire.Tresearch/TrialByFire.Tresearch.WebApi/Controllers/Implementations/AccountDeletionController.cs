
using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class AccountDeletionController: ControllerBase, IAccountDeletionController
    {
        private ISqlDAO SqlDAO { get; }
        private ILogService LogService { get; }

        private IAccountDeletionManager AccountDeletionManager { get; }

        public AccountDeletionController(ISqlDAO sqlDAO, ILogService logService, IAccountDeletionManager accountDeletionManager)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
            this.AccountDeletionManager = accountDeletionManager;
        }
        

        public string DeleteAccount()
        {
            string result = AccountDeletionManager.DeleteAccount();
            if (result.Equals("success"))
            {
                //LogService.CreateLog(DateTime.Now, "Server", principal.Identity.Name, "Account Deletion Successful");
                return result;
            }
            //might need to create log here
            return result;
            //if(result.Equals("success"))
        }

    }
}
