using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Account : IAccount
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string? Passphrase { get; }

        public string? AuthorizationLevel { get; set; }

        public bool? Status { get; set; }

        public bool? Confirmed { get; set; }

        public Account(string email, string username, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            if((email ?? username ?? passphrase ?? authorizationLevel) == null)
            {
                throw new AccountCreationFailedException("Data: Account creation failed. Null argument passed " +
                    "in for email or username or passphrase or authorization level.");
            }
            Email = email;
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = status;
            Confirmed = confirmed;
        }     
        
        public Account(string email, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            if ((email ?? passphrase ?? authorizationLevel) == null)
            {
                throw new AccountCreationFailedException("Data: Account creation failed. Null argument passed " +
                    "in for email or passphrase or authorization level.");
            }
            Email = email;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = status;
            Confirmed = confirmed;
        }
        public Account(string username, string passphrase)
        {
            if ((username ?? passphrase) == null)
            {
                throw new AccountCreationFailedException("Data: Account creation failed. Null argument passed " +
                    "in for username or passphrase.");
            }
            Username = username;
            Passphrase = passphrase;
        }
        public Account(string username)
        {
            if ((username) == null)
            {
                throw new AccountCreationFailedException("Data: Account creation failed. Null argument passed " +
                    "in for username.");
            }
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
