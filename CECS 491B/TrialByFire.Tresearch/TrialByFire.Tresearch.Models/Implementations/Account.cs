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
        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Passphrase { get; set; }

        public string? AuthorizationLevel { get; set; }

        public bool? Status { get; set; }

        public bool? Confirmed { get; set; }

        public Account(string email, string username, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            Email = email;
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = status;
            Confirmed = confirmed;
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
                    return Username.Equals(account.Username) || Email.Equals(account.Email);
                }
            }
            return false;
        }
    }


}
