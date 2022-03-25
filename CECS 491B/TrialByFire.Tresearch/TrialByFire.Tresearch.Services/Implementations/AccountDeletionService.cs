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


        Thread.CurrentPrincipal


        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService, CancellationToken cancellationToken = default)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
        }



        public string DeleteAccountAsync(CancellationToken cancellationToken = default)
        {
            try
            {

            }
            catch ()
            {

            }
            string results = SqlDAO.DeleteAccountAsync(cancellationToken);
            return results;
        }


    }
}
