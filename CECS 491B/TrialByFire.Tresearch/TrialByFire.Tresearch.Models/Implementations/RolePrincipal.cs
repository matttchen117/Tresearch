using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RolePrincipal : IRolePrincipal
    {
        private IRoleIdentity _roleIdentity;

        public IIdentity Identity { get { return _roleIdentity; } }

        public IRoleIdentity RoleIdentity { get { return _roleIdentity; } }

        public RolePrincipal(IRoleIdentity roleIdentity)
        {
            _roleIdentity = roleIdentity;
        }

        public bool IsInRole(string authorizationLevel)
        {
            return _roleIdentity.AuthorizationLevel.Equals(authorizationLevel);
        }
    }
}
