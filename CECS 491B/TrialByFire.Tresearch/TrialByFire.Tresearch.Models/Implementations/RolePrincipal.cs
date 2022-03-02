using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RolePrincipal
    {
        public IRoleIdentity _identity { get; }

        public RolePrincipal(IRoleIdentity identity)
        {
            _identity = identity;
        }

        public bool IsInRole(string role)
        {
            return _identity._role.Equals(role);
        }
    }
}
