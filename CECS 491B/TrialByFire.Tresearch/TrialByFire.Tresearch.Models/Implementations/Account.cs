using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Account : IAccount
    {
        public string Username { get; set; }

        public string? Passphrase { get; }

        public string AuthorizationLevel { get; set; }

        public bool? AccountStatus { get; set; }

        public bool? Confirmed { get; set; }

        public string? Token { get; set; }

        public Account(string username, string passphrase, string authorizationLevel, bool accountStatus, bool confirmed)
        {
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            AccountStatus = accountStatus;
            Confirmed = confirmed;
        }     
        
        public Account(string passphrase, string authorizationLevel, bool accountStatus, bool confirmed)
        {
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            AccountStatus = accountStatus;
            Confirmed = confirmed;
        }
        public Account(string username, string passphrase, string authorizationLevel)
        {
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
        }
        public Account(string username, string authorizationLevel)
        {
            Username = username;
            AuthorizationLevel = authorizationLevel;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj == null))
            {
                if(obj is IAccount)
                {
                    IAccount account = (IAccount)obj;
                    return Username.Equals(account.Username) && AuthorizationLevel.Equals(account.AuthorizationLevel);
                }
            }
            return false;
        }
    }


}
