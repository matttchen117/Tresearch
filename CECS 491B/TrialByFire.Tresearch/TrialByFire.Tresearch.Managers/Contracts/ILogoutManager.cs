﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ILogoutManager
    {
        public Task<string> LogoutAsync(CancellationToken cancellationToken = default);
    }
}
