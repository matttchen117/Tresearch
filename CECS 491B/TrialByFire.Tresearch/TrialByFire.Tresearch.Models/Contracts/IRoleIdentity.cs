﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IRoleIdentity : IIdentity
    {
        string AuthorizationLevel { get; }
        string UserHash { get; }
    }
}