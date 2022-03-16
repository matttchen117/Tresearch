using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountDeletionService : IAccountDeletionService
    {

        private ISqlDAO SqlDAO { get; }

        private ILogService LogService { get; }



        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
        }



        public string DeleteAccount()
        {
            string _results = SqlDAO.DeleteAccount();
            return _results;
        }


    }
}
