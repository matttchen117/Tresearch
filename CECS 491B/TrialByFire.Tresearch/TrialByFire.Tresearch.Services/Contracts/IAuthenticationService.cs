using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services.Contracts
{
    internal interface IAuthenticationService
    {
        ISqlDAO sqlDAO { get; set; }
        ILogService logService { get; set; }
    }
}
