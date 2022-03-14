using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class AccountDeletionManager : IAccountDeletionManager
    {
        private ISqlDAO SqlDAO { get; }
        private ILogService LogService { get; }
        private IAccountDeletionService AccountDeletionService { get; }
        private IRolePrincipal Principal { get; }
        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IAccountDeletionService accountDeletionService, IRolePrincipal principal)
        {
            SqlDAO = sqlDAO;
            LogService = logService;
            AccountDeletionService = accountDeletionService;
            Principal = principal;
        }


        public string DeleteAccount(IRolePrincipal principal)
        {
            string results = this.AccountDeletionService.DeleteAccount(principal);
            return results;
        }

    }
}
