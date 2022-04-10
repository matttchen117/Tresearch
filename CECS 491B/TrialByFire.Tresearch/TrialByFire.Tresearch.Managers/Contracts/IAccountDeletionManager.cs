using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using TrialByFire.Tresearch.Models.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IAccountDeletionManager
    {
        public Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken));

    }

}