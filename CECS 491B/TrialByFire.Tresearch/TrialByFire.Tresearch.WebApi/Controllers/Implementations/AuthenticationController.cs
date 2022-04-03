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
        private ILogService _logService { get; }
        private IAuthenticationManager _authenticationManager { get; }
        private IMessageBank _messageBank { get; }

        private BuildSettingsOptions _buildSettingsOptions { get; }

        public AuthenticationController(ISqlDAO sqlDAO, ILogService logService, 
            IAuthenticationManager authenticationManager, IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authenticationManager = authenticationManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }

        [HttpPost]
        [Route("test")]
        public string Test(string username, string otp, string authorizationLevel)
        {
            IRoleIdentity roleIdentity = new RoleIdentity(true, username, authorizationLevel);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Response.HttpContext.User = new ClaimsPrincipal(rolePrincipal);
            return HttpContext.User.Identity.Name;
        }

        [HttpPost]
        [Route("test2")]
        public string Test()
        {
            return HttpContext.User.IsInRole("user").ToString();
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
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, string authorizationLevel)
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
                //_logService.CreateLog(DateTime.Now.ToUniversalTime(), "Server", username, "Info", "Authentication Succeeded");
                split = result.Split(": ");
                return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
            }
            // {category}: {error message}
            //_logService.CreateLog(DateTime.Now.ToUniversalTime(), "Error", username, error[1], error[2]);
            split = result.Split(": ");
            return StatusCode(Convert.ToInt32(split[0]), split[2]);
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

        //
        // Summary:
        //     Generates the CookieOptions for the Cookie to be created for the User.
        //
        // Returns:
        //     The created CookieOptions.
        [ApiExplorerSettings(IgnoreApi = true)]
        private CookieOptions CreateCookieOptions()
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.IsEssential = true;
            cookieOptions.Expires = DateTime.Now.ToUniversalTime().AddDays(5);
            //cookieOptions.Path = "/";
            cookieOptions.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            cookieOptions.Secure = true;
            return cookieOptions;
            //Return CookieOptions, set actual Cookie in the caller code
            // For Readability
        }

    }
}
