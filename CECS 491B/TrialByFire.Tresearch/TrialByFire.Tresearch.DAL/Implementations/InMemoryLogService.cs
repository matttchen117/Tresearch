using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class InMemoryLogService : ILogService
    {
        private ISqlDAO _sqlDAO { get; }

        public InMemoryLogService(ISqlDAO inMemorySqlDAO)
        {
            _sqlDAO = inMemorySqlDAO;
        }

        public string CreateLog(DateTime timestamp, string level, string username, string category, string description)
        {
            return null;
        }
    }
}
