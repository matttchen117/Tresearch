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
        public string? Email { get; }

        private string? Username { get; }

        public string? Passphrase { get; }

        public string? AuthorizationLevel { get; }

        public bool? Status { get; }

        public bool? Confirmed { get; }

        public Account(string email, string username, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            Email = email;
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = false;
            Confirmed = false;
        }     
        
        public Account(string email, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            Email = email;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = status;
            Confirmed = confirmed;
        }
        public Account(string username, string passphrase)
        {
            Username = username;
            Passphrase = passphrase;
        }
        public Account(string username)
        {
            Username = username;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj == null))
            {
                if(obj is IAccount)
                {
                    IAccount account = (IAccount)obj;
                    return Username.Equals(account.username) || Email.Equals(account.email);
                }
            }
            return false;
        }
    }


}
