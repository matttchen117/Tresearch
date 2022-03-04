using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAccountDeletionService
    {
        string DeleteAccount(IPrincipal _rolePrincipal);
    }
}
