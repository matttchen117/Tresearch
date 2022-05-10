using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.Results;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    /// <summary>
    ///     AuthenticationController: Class that is part of the Controller abstraction layer that handles receiving and returning
    ///         HTTP response and requests for Authentication
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class AuthenticationController : Controller, IAuthenticationController
    {
        private ILogManager _logManager { get; }
        private IAuthenticationManager _authenticationManager { get; }
        private IMessageBank _messageBank { get; }

        private BuildSettingsOptions _buildSettingsOptions { get; }

        /// <summary>
        ///     public AuthenticationController():
        ///         Constructor for AuthenticationController class
        /// </summary>
        /// <param name="logManager">Manager object for Manager abstraction layer to handle business rules related to Logging</param>
        /// <param name="authenticationManager">Manager object for Manager abstraction layer to handle business rules related to Authentication</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="buildSettingsOptions">The settings/options</param>
        public AuthenticationController(ILogManager logManager, 
            IAuthenticationManager authenticationManager, IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _logManager = logManager;
            _authenticationManager = authenticationManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }

        /// <summary>
        ///     AuthenticateAsync:
        ///     
        /// </summary>
        /// <param name="username"></param>
        /// <param name="otp"></param>
        /// <param name="authorizationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel)
        {
            string[] split;
            string result;
            try
            {
                // Manager needs to check for guest and no token
                // need to pass in token
                List<string> results = await _authenticationManager.AuthenticateAsync(username, otp, authorizationLevel,
                    DateTime.Now.ToUniversalTime()).ConfigureAwait(false);
                result = results[0];
                split = result.Split(": ");
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).
                        ConfigureAwait(false)))
                {
                    if (_buildSettingsOptions.Environment.Equals("Test"))
                    {
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }
                    HttpContext.Response.Headers.Add(_buildSettingsOptions.AccessControlHeaderName, _buildSettingsOptions.JWTHeaderName);
                    HttpContext.Response.Headers.Add(_buildSettingsOptions.JWTHeaderName, results[1]);
                    await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server, 
                        await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                // REST - following http convention, always return proper status code
                // Return proper object (Ok objects for success, badrequest, internal server, or status code for fails

                // If fire and forget method, no way to test
                // No unit tests, just integration
                // always have await for async, regardless of being fire and forget
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
                // 500 - server error, nothing user did caused error,  
                // 400 - user caused error, 
                //These contain headers, status code does not
                //return new BadRequestResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) }; // 400 errors
                //return new InternalServerErrorResult() // 500 errors
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, DateTime now)
        {
            string[] split;
            string result = "";
            try
            {
                List<string> results = await _authenticationManager.AuthenticateAsync(username, 
                    otp, authorizationLevel, now).ConfigureAwait(false);
                result = results[0];
                split = result.Split(": ");
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).
                    ConfigureAwait(false)))
                {
                    if (_buildSettingsOptions.Environment.Equals("Test"))
                    {
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }
                    Response.Headers.Add(_buildSettingsOptions.AccessControlHeaderName, _buildSettingsOptions.JWTHeaderName);
                    Response.Headers.Add(_buildSettingsOptions.JWTHeaderName, results[1]);
                    // Enums good if possibilities are limited
                    // status codes
                    await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server,
                        await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
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
