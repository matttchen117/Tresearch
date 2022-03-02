using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    public interface ISqlDAO
    {
        string SqlConnectionString { get; set; }

        public bool CreateAccount(IAccount account);
        public bool CreateConfirmationLink(IConfirmationLink _confirmationlink);

    }
}
