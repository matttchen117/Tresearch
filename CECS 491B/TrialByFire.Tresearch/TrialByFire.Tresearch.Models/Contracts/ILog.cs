using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ILog
    {
        DateTime timestamp { get; }
        string level { get; }
        string username { get; }
        string category { get; }
        string description { get; }
    }
}
