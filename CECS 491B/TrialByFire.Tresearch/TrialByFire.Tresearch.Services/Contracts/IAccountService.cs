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
<<<<<<< HEAD
        public string CreatePreRegisteredAccount(IAccount account);
=======
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }

        public string CreatePreConfirmedAccount(IAccount account);
>>>>>>> f6d5c4b862e0619608144861e583cfd4d829f0fa

        public string CreateConfirmation(IAccount account, string baseUrl);
    }
}
