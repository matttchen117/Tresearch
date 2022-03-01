﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Account : IAccount
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Passphrase { get; set; }

        public string AuthorizationLevel { get; set; }

        public bool Status { get; set; }

        public bool Confirmed { get; set; }

        public Account(string email, string username, string passphrase, string authorizationLevel)
        {
            Email = email;
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = false;
            Confirmed = false;
        }     
    }


}
