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
        public string AuthenticationType => "Role";

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public string Role { get; }

        public RoleIdentity(bool isAuthenticated, string name, string role)
        {
            IsAuthenticated = isAuthenticated;
            Name = name;
            Role = role;
        }
    }
}
