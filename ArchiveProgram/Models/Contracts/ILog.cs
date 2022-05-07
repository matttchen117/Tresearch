using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProgram.Models.Contracts
{
    public interface ILog
    {
        public DateTime Timestamp { get; }
        public string Level { get; }
        public string UserHash { get; }
        public string Category { get; }
        public string Description { get; }
        public string Hash { get; }
    }
}
