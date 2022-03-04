using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountDeletionService : IAccountDeletionService
    {

        public SqlDAO _sqlDAO { get; set; }

        public ILogService _logService { get; set; }

        public string _result { get; set; }


        public AccountDeletionService(SqlDAO _sqlDAO, ILogService _logService)
        {
            this._sqlDAO = _sqlDAO;
            this._logService = _logService;
        }


        public string DeleteAccount(IPrincipal _rolePrincipal)
        {
            _result = _sqlDAO.DeleteAccount(_rolePrincipal);
        }

        public string DeleteAccount(IPrincipal _rolePrincipal)
        {
            string _results = _sqlDAO.DeleteAccount(_rolePrincipal);
            return _results;
        }
    }
}
