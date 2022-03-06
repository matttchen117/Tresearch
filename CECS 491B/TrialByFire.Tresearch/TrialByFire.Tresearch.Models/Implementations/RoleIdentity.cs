using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RoleIdentity : IRoleIdentity
    {
        public string AuthenticationType => "AuthorizationLevel";

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public string AuthorizationLevel { get; }

        public RoleIdentity(bool isAuthenticated, string name, string authorizationLevel)
        {
            IsAuthenticated = isAuthenticated;
            Name = name;
            AuthorizationLevel = authorizationLevel;
        }
    }
}
