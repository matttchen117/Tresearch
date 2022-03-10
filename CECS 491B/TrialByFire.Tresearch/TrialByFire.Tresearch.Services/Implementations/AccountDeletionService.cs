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

        private IRolePrincipal RolePrincipal { get; }


<<<<<<< HEAD

=======
>>>>>>> TestPammyMerge
        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService, IRolePrincipal rolePrincipal)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
            this.RolePrincipal = rolePrincipal;
        }



        public string DeleteAccount(IRolePrincipal rolePrincipal)
        {
            string _results = SqlDAO.DeleteAccount(rolePrincipal);
            return _results;
        }


    }
}
