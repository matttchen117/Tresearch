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
        public DateTime TimeStamp { get; }
        public string Level { get; }
        public string Username { get; }
        public string Category { get; }
        public string Description { get; }
        public Log(DateTime timestamp, string level, string username, string category, string description)
        {
            TimeStamp = timestamp;
            Level = level;
            Username = username;
            Category = category;
            Description = description;
        }

        public override string ToString()
        {
            return $"{TimeStamp} {Level} {Username} {Category} {Description}";
        }
    }
}
