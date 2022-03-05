using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAuthorizationService
    {
        string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredRole);
    }
}
