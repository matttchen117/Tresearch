using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RefreshSessionInput : IRefreshSessionInput
    {
        public string Username { get; set; }
        public string AuthorizationLevel { get; set; }
        public string? Token { get; set; }

        public RefreshSessionInput(string username, string authorizationLevel)
        {
            Username = username;
            AuthorizationLevel = authorizationLevel;
        }
    }
}
