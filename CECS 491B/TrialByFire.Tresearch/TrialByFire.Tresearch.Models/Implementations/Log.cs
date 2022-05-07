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
        public string UserHash { get; }
        public string Category { get; }
        public string Description { get; }
        public string Hash { get; }
        public Log(DateTime timestamp, string level, string userHash, string category, string description, 
            string hash)
        {
            Timestamp = timestamp;
            Level = level;
            UserHash = userHash;
            Category = category;
            Description = description;
            Hash = hash;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Timestamp: {0}, Level: {1}, UserHash: {2}, " +
                "Category: {3}, Description: {4}, Hash: {5}", Timestamp.ToString(), Level, UserHash, 
                Category, Description, Hash);
            return stringBuilder.ToString();
        }

        public override bool Equals(object? obj)
        {
            if(!(obj == null))
            {
                if(obj is Log)
                {
                    ILog log = (ILog)obj;
                    return Timestamp.Equals(log.Timestamp) && Level.Equals(log.Level) &&
                        UserHash.Equals(log.UserHash) && Category.Equals(log.Category) && 
                        Description.Equals(log.Description) && Hash.Equals(log.Hash);
                }
            }
            return false;
        }
    }
}
