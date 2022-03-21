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


        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IAccountDeletionService accountDeletionService)
        {
            SqlDAO = sqlDAO;
            LogService = logService;
            AccountDeletionService = accountDeletionService;
        }


        public string DeleteAccount()
        {
            string results = this.AccountDeletionService.DeleteAccount();
            return results;
        }

    }
}
