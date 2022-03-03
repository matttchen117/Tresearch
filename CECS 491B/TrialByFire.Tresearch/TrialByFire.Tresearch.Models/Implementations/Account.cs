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
        public string email { get; set; }

        public string username { get; set; }

        public string passphrase { get; set; }

        public string authorizationLevel { get; set; }

        public bool status { get; set; }

        public bool confirmed { get; set; }

        public Account(string email, string username, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            this.email = email;
            this.username = username;
            this.passphrase = passphrase;
            this.authorizationLevel = authorizationLevel;
<<<<<<< HEAD
            this.status = false;
            this.confirmed = false;
        }     

        public Account(string username, string passphrase)
        {
            this.email = "";
            this.username = username;
            this.passphrase = passphrase;
            this.authorizationLevel = "";
            this.status = false;
            this.confirmed = false;
=======
            this.status = status;
            this.confirmed = confirmed;
        }  
        
        public Account(string email, string passphrase, string authorizationLevel, bool status, bool confirmed)
        {
            this.email = email;
            this.passphrase = passphrase;
            this.authorizationLevel = authorizationLevel;
            this.status = status;
            this.confirmed = confirmed;
>>>>>>> f6d5c4b862e0619608144861e583cfd4d829f0fa
        }
    }


}
