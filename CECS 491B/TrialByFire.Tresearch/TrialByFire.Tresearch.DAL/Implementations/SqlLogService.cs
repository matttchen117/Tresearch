using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlLogService : ILogService
    {
        private  ISqlDAO _sqlDAO { get; }

        public SqlLogService(ISqlDAO sqlDAO)
        {
            _sqlDAO = sqlDAO;
        }

        public async Task<string> CreateLog(DateTime timestamp, string level, string username, 
            string category, string description, CancellationToken cancellationToken = default)
        {
            try
            {
                ILog log = new Log(timestamp, level, username, category, description);
                return await _sqlDAO.StoreLogAsync(log, cancellationToken).ConfigureAwait(false);
            }catch (Exception ex)
            {
                // Revise
                return ex.Message;
            }
        }
    }
}
