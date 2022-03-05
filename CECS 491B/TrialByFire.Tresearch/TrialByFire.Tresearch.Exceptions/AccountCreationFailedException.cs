using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountCreationFailedException : Exception
    {
        public AccountCreationFailedException()
        {
        }

        public AccountCreationFailedException(string message)
            : base(message)
        {
        }

        public AccountCreationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
