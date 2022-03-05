using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        public AuthorizationService(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
        }

        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredRole)
        {
            return _sqlDAO.VerifyAuthorized(rolePrincipal, requiredRole);
        }
    }
}
