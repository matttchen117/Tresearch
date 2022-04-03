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
        private IMailService _mailService { get; set; }
        private IRegistrationService _registrationService { get; }

        private IValidationService _validationService { get; }

        private IMessageBank _messageBank { get; }

        public IRegistrationManager _registrationManager { get; }

        public RegistrationController(ISqlDAO sqlDAO, ILogService logService, IRegistrationService registrationService, IMailService mailService,
                                      IMessageBank messageBank, IValidationService validation, IRegistrationManager registrationManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _registrationService = registrationService;
            _mailService = mailService;
            _messageBank = messageBank;
            _validationService = validation;
            _registrationManager = registrationManager;
        }



        [HttpPost("register")]
        public IActionResult RegisterAccount(string email, string passphrase)
        {
            
            List<string> results = new List<string>();
            try
            {
                results.AddRange(_registrationManager.CreatePreConfirmedAccount(email, passphrase));
                
                //Assign Status Codes
                if(results.Last() == "Success - Registration Manager created account")
                {
                    results.Add("Success - Account successfully created");
                } else
                {
                    if (results.First() == "Failed - Account already exists in database")
                    {
                        results.Add("Failed - Account already exists");
                        return StatusCode(409, results.Last());
                    } else if (results.First() == "Failed - Account not created in database")
                    {

                    }
                }




                if (results.Last() == "Success - Registration Manager created account")
                    results.Add("Success - Registration Controller created account");
                else
                    results.Add("Failed - Registration Controller could not create account");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Registration Manager " + ex);
            }
            string r = "";
            for (int i = 0; i < results.Count(); i++)
            {
                r += "\t" + results[i];
            }

            results.Add(r);
            //SendConfirmation(email);

            _logService.StoreLogAsync(DateTime.Now.ToUniversalTime(), "Info", email, "Business", results.Last());
            return Ok(results.Last());
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
                _logService.StoreLogAsync(DateTime.Now.ToUniversalTime(), "Info", email, "Business", results.Last());
            else
                _logService.StoreLogAsync(DateTime.Now.ToUniversalTime(), "Info", email, "Error", results.Last());
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
                _logService.StoreLogAsync(DateTime.Now.ToUniversalTime(), "Info", results.First(), "Business", results.Last());
            else
                _logService.StoreLogAsync(DateTime.Now.ToUniversalTime(), "Info", results.First(), "Error", results.Last());
            return Ok(results.Last());
        }
    }
}
