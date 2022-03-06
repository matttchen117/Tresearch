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
    public class RegistrationController : Controller, IRegistrationController
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
        public string RegisterAccount([FromBody]IAccount account)
        {
            List<string> results = new List<string>();
            string email = account.Email;
            string passphrase = account.Passphrase;
            try
            {
                results.AddRange(_registrationManager.CreatePreConfirmedAccount(email, passphrase));
                if (results.Last() == "Success - Registration Manager created account")
                    results.Add("Success - Registration Controller created account");
                else
                    results.Add("Failed - Registration Controller could not create account");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Registration Manager " + ex);
            }
            for (int i = 0; i < results.Count(); i++)
            {
                Console.WriteLine(results[i]);
            }

            SendConfirmation(email);

            _logService.CreateLog(DateTime.Now, "Info", email, "Business", results.Last());
            return results.Last();
        }

        [HttpPost("confirmation")]
        public string SendConfirmation(string email)
        {
            IAccount account = new Account();
            account.Email = email;
            account.Username = email;
            List<string> results = new List<string>();
            bool error = false;
            string baseUrl = "www.tresearch.systems/RegistrationController";
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
            for (int i = 0; i < results.Count(); i++)
            {
                Console.WriteLine(results[i]);
            }
            if (!error)
                _logService.CreateLog(DateTime.Now, "Info", email, "Business", results.Last());
            else
                _logService.CreateLog(DateTime.Now, "Info", email, "Error", results.Last());
            return results.Last();

        }

        [HttpPost("confirm")]
        public string ConfirmAccount(string url)
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
            for (int i = 0; i < results.Count(); i++)
            {
                Console.WriteLine(results[i]);
            }
            if (!error)
                _logService.CreateLog(DateTime.Now, "Info", results.First(), "Business", results.Last());
            else
                _logService.CreateLog(DateTime.Now, "Info", results.First(), "Error", results.Last());
            return results.Last();
        }
    }
}
