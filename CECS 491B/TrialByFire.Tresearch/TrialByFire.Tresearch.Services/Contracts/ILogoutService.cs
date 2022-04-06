using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ILogoutService
    {
        public Task<string> LogoutAsync(CancellationToken cancellationToken = default);
    }
}
