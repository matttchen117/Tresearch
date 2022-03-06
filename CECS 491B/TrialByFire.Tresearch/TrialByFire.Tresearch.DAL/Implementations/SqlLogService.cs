using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlLogService : ILogService
    {
        private  ISqlDAO _sqlDAO { get; }

        public SqlLogService(ISqlDAO sqlDAO)
        {
            _sqlDAO = sqlDAO;
        }

        public string CreateLog(DateTime timestamp, string level, string username, string category, string description)
        {
            return null;
        }
    }
}
