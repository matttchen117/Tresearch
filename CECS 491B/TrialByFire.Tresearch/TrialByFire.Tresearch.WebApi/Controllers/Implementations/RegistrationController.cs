using Microsoft.AspNetCore.Mvc;
using System.Runtime.dll;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;



namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class RegistrationController: Controller, IRegistrationController
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private IAccountManager _accountManager;
        public void RegisterAccount(string email, string passphrase)
        {
            
            
        }


    }
}
