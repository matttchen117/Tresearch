﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<List<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default);
        Task<List<string>> RefreshSessionAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default);
    }
}
