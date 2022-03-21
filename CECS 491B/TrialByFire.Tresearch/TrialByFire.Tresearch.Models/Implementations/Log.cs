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
        public DateTime Timestamp { get; }
        public string Level { get; }
        public string Username { get; }
        public string Category { get; }
        public string Description { get; }
        public Log(DateTime timestamp, string level, string username, string category, string description)
        {
            Timestamp = timestamp;
            Level = level;
            Username = username;
            Category = category;
            Description = description;
        }
    }
}
