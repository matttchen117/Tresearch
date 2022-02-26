using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    internal interface ILogService
    {
        ISqlDAO sqlDAO { get; }

        string CreateLog(DateTime timestamp, string level, string username, string category, string description);

    }
}
