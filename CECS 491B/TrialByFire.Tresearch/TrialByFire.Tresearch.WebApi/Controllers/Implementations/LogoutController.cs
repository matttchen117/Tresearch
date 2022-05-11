using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    /// <summary>
    ///     LogoutController: Class that is part of the Controller abstraction layer that handles receiving and returning
    ///         HTTP response and requests for Logout
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class LogoutController : Controller, ILogoutController
    {
        private ILogManager _logManager { get; }
        private IMessageBank _messageBank { get; }

        private ILogoutManager _logoutManager { get; }

        private BuildSettingsOptions _options { get; }
        /// <summary>
        ///     public LogoutController():
        ///         Constructor for LogoutController class
        /// </summary>
        /// <param name="logManager">Manager object for Manager abstraction layer to handle business rules related to Logging</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="logoutManager">Manager object for Manager abstraction layer to handle business rules related to Logout</param>
        /// <param name="options">The settings/options</param>
        public LogoutController(ILogManager logManager, IMessageBank messageBank, 
            ILogoutManager logoutManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _logManager = logManager;
            _messageBank = messageBank;
            _logoutManager = logoutManager;
            _options = options.Value;
        }

        /// <summary>
        ///     LogoutAsync:
        ///         Async method that handles receiving HTTP requests for Logout operation and returning the results
        /// </summary>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                string result = await _logoutManager.LogoutAsync().ConfigureAwait(false);

                string[] split = result.Split(": ");
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess)
                    .ConfigureAwait(false)))
                {
                    // Don't modify header's if in Test environment
                    if (_options.Environment.Equals("Test"))
                    {
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }

                    HttpContext.User = null;
                    Response.Headers.Remove(_options.JWTHeaderName);
                    await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server, 
                        await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess).ConfigureAwait(false));
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                if (Enum.TryParse(split[1], out ILogManager.Categories category))
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category, split[2]);
                }
                else
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (OperationCanceledException tce)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + tce.Message);
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level:
                    ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await
                    _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false)
                    + ex.Message);
                return StatusCode(600, ex.Message);
            }
        }
    }
}
