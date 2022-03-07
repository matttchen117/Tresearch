
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

        private IRolePrincipal RolePrincipal { get; }

        public AccountDeletionController(ISqlDAO sqlDAO, ILogService logService, IAccountDeletionManager accountDeletionManager, IRolePrincipal rolePrincipal)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
            this.AccountDeletionManager = accountDeletionManager;
            this.RolePrincipal = rolePrincipal;
            
        }
        

        public string DeleteAccount(IRolePrincipal rolePrincipal)
        {
            string result = AccountDeletionManager.DeleteAccount(rolePrincipal);
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
