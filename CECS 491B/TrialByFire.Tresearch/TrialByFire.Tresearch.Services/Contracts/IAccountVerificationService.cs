﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAccountVerificationService
    {
        public Task<string> VerifyAccountAsync(IAccount account, CancellationToken cancellationToken = default);
    }
}