using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
<<<<<<< HEAD
<<<<<<< HEAD
    public interface IAccount
=======
    public class IAccount
>>>>>>> 146c85545acdf57f7cd543f2a28f1888e13ee898
    {
    }
}
=======
    public interface IAccount
    {
        string email { get; set; }
        
        string username { get; set; }

        string passphrase { get; set; }

        string authorizationLevel { get; set; }

        bool status { get; set; }

        bool confirmed { get; set; }
    }
}
>>>>>>> origin/Ian's-Branch
