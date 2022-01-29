using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountServiceCreationFailedException : Exception
    {
        public AccountServiceCreationFailedException() { }

        public AccountServiceCreationFailedException(string message) : base(message) { }

        public AccountServiceCreationFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
