using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IAccount
    {
        string email { get; set; }
        
        string username { get; set; }

        public string? Passphrase { get; }

        string? AuthorizationLevel { get; set; }

        string authorizationLevel { get; set; }

        bool status { get; set; }

        bool confirmed { get; set; }
    }
}
