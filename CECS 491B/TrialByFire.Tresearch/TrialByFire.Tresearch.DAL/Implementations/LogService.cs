using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class LogService : ILogService
    {
        public ISqlDAO sqlDAO { get; }

        public LogService(ISqlDAO sqlDAO)
        {
            this.sqlDAO = sqlDAO;
        }

        public string CreateLog(DateTime timestamp, string level, string username, string category, string description)
        {
            throw new NotImplementedException();
        }
    }
}
