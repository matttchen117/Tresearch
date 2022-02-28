using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;



namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAccountService
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }

        public string CreatePreRegisteredAccount(IAccount account);

    }
}
