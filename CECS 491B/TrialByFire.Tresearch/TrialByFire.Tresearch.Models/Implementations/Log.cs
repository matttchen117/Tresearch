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
        public string Hash { get; }
        public Log(DateTime timestamp, string level, string username, string category, string description, 
            string hash)
        {
            Timestamp = timestamp;
            Level = level;
            Username = username;
            Category = category;
            Description = description;
            Hash = hash;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj == null))
            {
                if(obj is Log)
                {
                    ILog log = (ILog)obj;
                    return Timestamp.Equals(log.Timestamp) && Level.Equals(log.Level) && 
                        Username.Equals(log.Username) && Category.Equals(log.Category) && 
                        Description.Equals(log.Description) && Hash.Equals(log.Hash);
                }
            }
            return false;
        }
    }
}
