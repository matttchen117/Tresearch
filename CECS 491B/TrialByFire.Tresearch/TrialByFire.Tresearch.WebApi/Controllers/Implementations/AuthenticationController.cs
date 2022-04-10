using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    // Summary:
    //     A controller class for Authenticating the User.
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class AuthenticationController : Controller, IAuthenticationController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogManager _logManager { get; }
        private IAuthenticationManager _authenticationManager { get; }
        private IMessageBank _messageBank { get; }

        private BuildSettingsOptions _buildSettingsOptions { get; }

        public AuthenticationController(ISqlDAO sqlDAO, ILogManager logManager, 
            IAuthenticationManager authenticationManager, IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _sqlDAO = sqlDAO;
            _logManager = logManager;
            _authenticationManager = authenticationManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }

        //
        // Summary:
        //     Entry point for Authentication requests and creates the Cookie for the User on success.
        //
        // Parameters:
        //   username:
        //     The username entered by the User attempting to Authenticate.
        //   otp:
        //     The otp entered by the User attempting to Authenticate.
        //   authorizationLevel:
        //     The selected authorization level for the Account that the User is trying Authenticate for.
        //
        // Returns:
        //     The result of the operation with any status codes if applicable.
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, CancellationToken cancellationToken = default)
        {
            string[] split;
            string result;
            try
            {
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
                    _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server, 
                        await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                if (Enum.TryParse(split[1], out ILogManager.Categories category))
                {
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category, split[2]);
                }
                else
                {
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (OperationCanceledException tce)
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + tce.Message);
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level:
                    ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await
                    _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false)
                    + ex.Message);
                return StatusCode(600, ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, DateTime now, CancellationToken cancellationToken = default)
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
                    _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server,
                        await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                if (Enum.TryParse(split[1], out ILogManager.Categories category))
                {
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category, split[2]);
                }
                else
                {
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (OperationCanceledException tce)
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + tce.Message);
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: 
                    ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await 
                    _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) 
                    + ex.Message);
                return StatusCode(600, ex.Message);
            }
        }
    }
}
