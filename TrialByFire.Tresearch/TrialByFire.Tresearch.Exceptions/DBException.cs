using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class DBException : Exception
    {
        public DBException() { }

        public DBException(string message) : base(message) { }

        public DBException(string message, Exception inner) : base(message, inner) { }
    }
}
