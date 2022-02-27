using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    internal interface ISqlDAO
    {
        string sqlConnectionString { get; set; }
    }
}
