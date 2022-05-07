using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiveProgram.Models.Contracts;

namespace ArchiveProgram.Models.Implementations
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

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is Log)
                {
                    ILog log = (ILog)obj;
                    return Timestamp.Equals(log.Timestamp) && Level.Equals(log.Level) &&
                        UserHash.Equals(log.UserHash) && Category.Equals(log.Category) &&
                        Description.Equals(log.Description) && Hash.Equals(log.Hash);
                }
            }
            return false;
        }
        public override string ToString()
        {
            return $"{Timestamp} {Level} {UserHash} {Category} {Description} {Hash}";
        }
    }
}
