using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountDeletionFailedException : Exception
    {

        public AccountDeletionFailedException()
        {
        }

        public AccountDeletionFailedException(string message)
            : base(message)
        {
        }

        public AccountDeletionFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
