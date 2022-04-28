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
        public AuthorizationService(ISqlDAO sqlDAO)
        {
            _sqlDAO = sqlDAO;
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
        public async Task<bool> VerifyAuthorizedAsync(string requiredAuthLevel, 
            string identity, CancellationToken cancellationToken = default)
        {
            if(Thread.CurrentPrincipal != null)
            {
                if(!requiredAuthLevel.Equals(""))
                {
                    return Thread.CurrentPrincipal.IsInRole(requiredAuthLevel)
                    || Thread.CurrentPrincipal.IsInRole("admin");
                }
                else if(!identity.Equals(""))
                {
                    return Thread.CurrentPrincipal.Identity.Name.Equals(identity);
                }
            }
            return false;
        }
    }
}