using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Log : ILog
    {
        public DateTime timestamp { get; }
        public string level { get; }
        public string username { get; }
        public string category { get; }
        public string description { get; }
        public Log(DateTime timestamp, string level, string username, string category, string description)
        {
            this.timestamp = timestamp;
            this.level = level;
            this.username = username;
            this.category = category;
            this.description = description;
        }
    }
}
