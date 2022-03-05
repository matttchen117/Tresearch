using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IAccount
    {
        string? Email { get; set; }
        
        string? Username { get; set; }

        public string? Passphrase { get; }

        string? AuthorizationLevel { get; set; }

        bool? Status { get; set; }

        bool? Confirmed { get; set; }
    }
}
