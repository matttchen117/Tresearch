using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ILog
    {
        public DateTime TimeStamp { get; }
        public string Level { get; }
        public string Username { get; }
        public string Category { get; }
        public string Description { get; }
    }
}
