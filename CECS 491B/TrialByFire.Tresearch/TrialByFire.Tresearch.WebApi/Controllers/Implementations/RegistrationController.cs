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
        private IMessageBank _messageBank { get; }
        private string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm/guid=";

        public RegistrationController(ISqlDAO sqlDAO, ILogService logService, IRegistrationManager registrationManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _registrationManager = registrationManager;
            _messageBank = messageBank;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccountAsync(string email, string passphrase)
        {
            try
            {
                string result = await _registrationManager.CreateAndSendConfirmationAsync(email, passphrase, "user", baseUrl).ConfigureAwait(false);
                string[] split;
                split = result.Split(":");
                if(result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccountAsync(string guid)
        {
            try
            {
                string result = await _registrationManager.ConfirmAccountAsync(guid).ConfigureAwait(false);
                string[] split;
                split = result.Split(":");
                if (result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
