using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAccountDeletionService
    {
        public Task<List<string>> DeleteAccountAsync(CancellationToken cancellationToken = default);
    }
}
