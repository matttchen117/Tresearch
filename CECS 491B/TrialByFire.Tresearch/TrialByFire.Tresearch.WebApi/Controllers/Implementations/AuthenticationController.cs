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
            List<string> results = await _authenticationManager.AuthenticateAsync(username, otp, authorizationLevel, 
                DateTime.Now.ToUniversalTime()).ConfigureAwait(false);
            string result = results[0];
            if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).
                    ConfigureAwait(false)))
            {
                if (_buildSettingsOptions.Environment.Equals("Test"))
                {
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                HttpContext.Response.Headers.Add("Authorization", results[1]);
                _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info, username, 
                    authorizationLevel, category: ILogManager.Categories.Server, "Authentication Succeeded");
                split = result.Split(": ");
                return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
            }
            // {category}: {error message}
            split = result.Split(": ");
            if(Enum.TryParse(split[1], out ILogManager.Categories category))
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error, username, authorizationLevel,
                category, split[2]);
            }
            else
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error, username, authorizationLevel,
                category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
            }
            return StatusCode(Convert.ToInt32(split[0]), split[2]);
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
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).
                    ConfigureAwait(false)))
                {
                    if (_buildSettingsOptions.Environment.Equals("Test"))
                    {
                        split = result.Split(": ");
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }
                    //Response.Cookies.Append("TresearchAuthenticationCookie", results[1], CreateCookieOptions());
                    Response.Headers.Add("Access-Control-Allow-Headers", "Authorization");
                    Response.Headers.Add("Authorization", results[1]);
                    //_logService.CreateLog(DateTime.Now.ToUniversalTime(), "Server", username, "Info", "Authentication Succeeded");
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
            }
            catch (OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            // {category}: {error message}
            //_logService.CreateLog(DateTime.Now.ToUniversalTime(), "Error", username, error[0], error[1]);
            split = result.Split(": ");
            return StatusCode(Convert.ToInt32(split[0]), split[2]);
        }

        [HttpPost]
        [Route("refreshSession")]
        public async Task<IActionResult> RefreshSessionAsync(CancellationToken cancellationToken = default)
        {
            List<string> results = await _authenticationManager.RefreshSessionAsync().ConfigureAwait(false);
            string[] split;
            string result = results[0];
            if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.refreshSessionSuccess).
                    ConfigureAwait(false)))
            {
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                HttpContext.Response.Headers.Add("Authorization", results[1]);
                _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                    Thread.CurrentPrincipal.Identity.Name,
                    (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel, category: ILogManager.Categories.Server, "Authentication Succeeded");
                split = result.Split(": ");
                return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
            }
            // {category}: {error message}
            split = result.Split(": ");
            if (Enum.TryParse(split[1], out ILogManager.Categories category))
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error, Thread.CurrentPrincipal.Identity.Name,
                    (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel, category, split[2]);
            }
            else
            {
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error, Thread.CurrentPrincipal.Identity.Name,
                    (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel, category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
            }
            return StatusCode(Convert.ToInt32(split[0]), split[2]);
        }
    }
}
