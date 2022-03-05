using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Exceptions
{
    public class RolePrincipalCreationFailedException : Exception
    {
        public RolePrincipalCreationFailedException()
        {
        }

        public RolePrincipalCreationFailedException(string message)
            : base(message)
        {
        }

        public RolePrincipalCreationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
