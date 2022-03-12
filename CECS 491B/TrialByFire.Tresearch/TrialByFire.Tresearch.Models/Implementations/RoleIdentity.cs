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

        public string? Name { get; }

        public string Username { get; set; }

        public string AuthorizationLevel { get; set; }

        public RoleIdentity(bool isAuthenticated, string username, string authorizationLevel)
        {
            IsAuthenticated = isAuthenticated;
            Username = username;
            AuthorizationLevel = authorizationLevel;
        }
    }
}
