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
    // Summary:
    //     A service class for Authorizing the User
    public class AuthorizationService : IAuthorizationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        public AuthorizationService(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
        }

        //
        // Summary:
        //     Verifies the Authorization Level of the current Principal of the User is 
        //  
        //
        // Parameters:
        //   requiredAuthLevel:
        //     The requried Authorization Level to perform the operation
        //
        // Returns:
        //     The result of the verification process.
        public async Task<string> VerifyAuthorizedAsync(string requiredAuthLevel, 
            CancellationToken cancellationToken)
        {
            return await _sqlDAO.VerifyAuthorizedAsync(requiredAuthLevel, cancellationToken);
        }
    }
}