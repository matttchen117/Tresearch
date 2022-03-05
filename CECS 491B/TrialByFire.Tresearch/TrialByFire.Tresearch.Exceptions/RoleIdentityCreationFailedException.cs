using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class RoleIdentityCreationFailedException : Exception
    {
        public RoleIdentityCreationFailedException()
        {
        }

        public RoleIdentityCreationFailedException(string message)
            : base(message)
        {
        }

        public RoleIdentityCreationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
