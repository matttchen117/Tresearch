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

    /// <summary>
    ///     Controller class for registration. Handles posts, gets etc. from client
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase, IRegistrationController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IRegistrationManager _registrationManager { get; }
        private IMessageBank _messageBank { get; }
        private string baseUrl = "https://trialbyfiretresearch.azurewebsites.net/Register/Confirm/guid=";

        /// <summary>
        ///     Class constructor
        /// </summary>
        /// <param name="sqlDAO"> Sqldao performs database functions</param>
        /// <param name="logService">log service</param>
        /// <param name="registrationManager">Manager</param>
        /// <param name="messageBank">Message bank holds error and success enums</param>
        public RegistrationController(ISqlDAO sqlDAO, ILogService logService, IRegistrationManager registrationManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _registrationManager = registrationManager;
            _messageBank = messageBank;
        }


        /// <summary>
        ///     RegisterAccountAsync(email, passphrase)
        ///         Post request registering account. 
        /// </summary>
        /// <param name="email">string email of user</param>
        /// <param name="passphrase">string passphrase of user</param>
        /// <returns>Status code and value</returns>
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
                    split = result.Split(":");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///  ConfirmAccountAsync(guid)
        ///     Post request confirming account based on guid passed in
        /// </summary>
        /// <param name="guid">Unique identifier to each confirmation link</param>
        /// <returns>Status scode and value</returns>
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
                    split = result.Split(":");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///     ResendConfirmationLinkAsync(guid)
        ///         Post request resending confirmation link. Used when inactive link was used
        /// </summary>
        /// <param name="guid">Unique identifier for confirmation link</param>
        /// <returns>Statuscode and value</returns>
        [HttpPost("resend")]
        public async Task<IActionResult> ResendConfirmationLinkAsync(string guid)
        {
            try
            {
                string result = await _registrationManager.ResendConfirmation(guid, baseUrl).ConfigureAwait(false);
                string[] split;
                split = result.Split(":");
                if (result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    split = result.Split(":");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch(OperationCanceledException)
            {
                string result = await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
