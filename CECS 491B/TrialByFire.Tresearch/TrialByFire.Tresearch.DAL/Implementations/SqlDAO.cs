using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    class SqlDAO : ISqlDAO
    {
        public string sqlConnectionString { get; set; }

        public SqlDAO(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }
    }
}
