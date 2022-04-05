﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IAccount
    {
        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Passphrase { get; }

        public string? AuthorizationLevel { get; set; }

        public bool? AccountStatus { get; set; }

        public bool? Confirmed { get; set; }

        public string? Token { get; set; }

    }
}
