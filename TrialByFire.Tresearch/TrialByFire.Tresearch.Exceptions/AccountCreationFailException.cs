using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountCreationFailException : Exception
    {
        public AccountCreationFailException() { }

        public AccountCreationFailException(string message) : base(message) { }

        public AccountCreationFailException(string message, Exception inner) : base(message, inner) { }
    }
}
