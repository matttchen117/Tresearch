using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase, IRegistrationController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IRegistrationManager _registrationManager { get; }

        public RegistrationController(ISqlDAO sqlDAO, ILogService logService, IRegistrationManager registrationManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _registrationManager = registrationManager;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount(string email, string passphrase)
        {
            try
            {
                string result = await _registrationManager.CreatePreConfirmedAccount(email, passphrase).ConfigureAwait(false);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("confirmation")]
        public IActionResult SendConfirmation(string email)
        {
            IAccount account = new Account();
            account.Email = email;
            account.Username = email;
            List<string> results = new List<string>();
            bool error = false;
            string baseUrl = "https://localhost:7010/Registration/confirmation?";
            try
            {
                results.AddRange(_registrationManager.SendConfirmation(account.Email, baseUrl));
                if (results.Last() == "Success - Registration Manager sent email confirmation")
                    results.Add("Success - Registration Controller sent email confirmation");
                else
                {
                    error = true;
                    results.Add("Failed - Registration Controller could not create account");

                }

            }
            catch (Exception ex)
            {
                error = true;
                results.Add("Failed - Registration Controller " + ex);
            }

            if (!error)
                _logService.CreateLog(DateTime.Now, "Info", email, "Business", results.Last());
            else
                _logService.CreateLog(DateTime.Now, "Info", email, "Error", results.Last());
            return Ok(results.Last());

        }

        [HttpPost("confirm")]
        public IActionResult ConfirmAccount(string url)
        {
            List<string> results = new List<string>();
            bool error = false;
            try
            {
                results.AddRange(_registrationManager.ConfirmAccount(url));
                if (results.Last() == "Success - Registration Manager confirmed account")
                    results.Add("Success - Registration Controller confirmed account");
                else
                {
                    error = true;
                    results.Add("Failed - Registration Controller could not confirm account");
                }

            }
            catch (Exception ex)
            {
                error = true;
                results.Add("Failed - Registration Controller " + ex);
            }
            if (!error)
                _logService.CreateLog(DateTime.Now, "Info", results.First(), "Business", results.Last());
            else
                _logService.CreateLog(DateTime.Now, "Info", results.First(), "Error", results.Last());
            return Ok(results.Last());
        }
    }
}
