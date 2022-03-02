using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
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
            string code ="";
            try
            {
                code = _accountManager.CreatePreConfirmedAccount(email, passphrase);
            } catch
            {
                
            }
            _logService.CreateLog(DateTime.Now, "Info", email, "Business", code);

        }
        [HttpGet]
        public void SendConfirmation(IAccount account)
        {
            string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            _accountManager.SendConfirmation(account, baseUrl);
        }

        [HttpGet]
        public void ConfirmAccount(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                // Return url is null or ""
            }
        }
    }
}
