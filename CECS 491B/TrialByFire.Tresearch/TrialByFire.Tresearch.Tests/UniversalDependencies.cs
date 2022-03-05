using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class UniversalDependencies
    {
        public ISqlDAO sqlDAO { get; set; }
        public ILogService logService { get; set; }

        public UniversalDependencies()
        {
            sqlDAO = new SqlDAO();
            logService = new SqlLogService(sqlDAO);
        }

        // or do like this?
        /*
         * public UniversalDependencies(ISqlDAO sqlDAO, ILogService logService)
        {
            this.sqlDAO = sqlDAO
            this.logService = logService;
        }
}
}
