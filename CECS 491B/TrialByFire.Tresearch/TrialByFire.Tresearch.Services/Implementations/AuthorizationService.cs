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
    /// <summary>
    ///     AuthorizationService: Class that is part of the Service abstraction layer that performs services related to Authorization
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private ISqlDAO _sqlDAO { get; }

        /// <summary>
        ///     public AuthorizationService():
        ///         Constructor for AuthorizationService class
        /// </summary>
        /// <param name="sqlDAO">SQL Data Access Object to interact with the database</param>
        public AuthorizationService(ISqlDAO sqlDAO)
        {
            _sqlDAO = sqlDAO;
        }

        /// <summary>
        ///     public VerifyAuthorizedAsync():
        ///         Checks if user is authorized by role or by identity
        /// </summary>
        /// <param name="requiredAuthLevel">The authrozation level required</param>
        /// <param name="identity">The identity of the user</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The result of the operation</returns>
        public async Task<bool> VerifyAuthorizedAsync(string requiredAuthLevel, 
            string identity, CancellationToken cancellationToken = default)
        {
            if(Thread.CurrentPrincipal != null)
            {
                if(!requiredAuthLevel.Equals(""))
                {
                    return Thread.CurrentPrincipal.IsInRole(requiredAuthLevel) || Thread.CurrentPrincipal.IsInRole("admin");
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