using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProgram
{
    public class Settings
    {
        public string SqlConnectionString { get; set; } = String.Empty;
        public string Source { get; set; } = String.Empty;
        public string Destination { get; set; } = String.Empty;
    }
}
