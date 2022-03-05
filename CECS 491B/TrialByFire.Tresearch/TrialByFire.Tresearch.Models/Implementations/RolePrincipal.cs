using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RolePrincipal : IRolePrincipal
    {
        private IRoleIdentity _roleIdentity;

        public IIdentity Identity { get { return _roleIdentity; } }

        public RolePrincipal(IRoleIdentity roleIdentity)
        {
            if(roleIdentity == null)
            {
                throw new RolePrincipalCreationFailedException("Data: Role Principal creation failed. Null argument " +
                    "passed in for role identity.");
            }
            _roleIdentity = roleIdentity;
        }

        public bool IsInRole(string role)
        {
            return _roleIdentity.Role.Equals(role);
        }
    }
}
