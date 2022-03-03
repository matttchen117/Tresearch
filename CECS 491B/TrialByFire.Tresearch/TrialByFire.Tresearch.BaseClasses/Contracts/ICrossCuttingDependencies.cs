using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.BaseClasses.Contracts
{
    public interface ICrossCuttingDependencies
    {
        ISqlDAO sqlDAO { get; set; }
        ILogService logService { get; set; }
        IAuthenticationService authenticationService { get; set; }
    }
}
