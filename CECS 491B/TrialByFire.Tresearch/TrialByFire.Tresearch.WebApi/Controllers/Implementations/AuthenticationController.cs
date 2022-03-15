using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller, IAuthenticationController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IAuthenticationManager _authenticationManager { get; }
        private IMessageBank _messageBank { get; }

        private string _username { get; set; }


        public AuthenticationController(ISqlDAO sqlDAO, ILogService logService, 
            IAuthenticationManager authenticationManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authenticationManager = authenticationManager;
            _messageBank = messageBank;
            _username = "guest";
        }

        [HttpPost]
        [Route("test")]
        public string Test(string username, string otp, string authorizationLevel)
        {
            return $"success: {username} + {otp} + {authorizationLevel}";
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, string authorizationLevel)
        {
            _username = username;
            List<string> results = await _authenticationManager.AuthenticateAsync(username, otp, authorizationLevel, DateTime.Now);
            string result = results[0];
            if (result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.IsEssential = true;
                cookieOptions.Expires = DateTime.Now.AddDays(5);
                cookieOptions.Path = "/";
                cookieOptions.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                cookieOptions.Secure = true;
                Response.Cookies.Append("TresearchAuthenticationCookie", results[1], cookieOptions);
                //result = await CreateCookieAsync(results[1]);
                if (result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    //_logService.CreateLog(DateTime.Now, "Server", username, "Info", "Authentication Succeeded");
                    return new OkResult();
                }
            }
            // {category}: {error message}
            //_logService.CreateLog(DateTime.Now, "Error", username, error[1], error[2]);
            string[] error = result.Split(": ");
            return StatusCode(Convert.ToInt32(error[0]), error[2]);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AuthenticateAsync(string username, string otp, string authorizationLevel, DateTime now)
        {
            _username = username;
            List<string> results = await _authenticationManager.AuthenticateAsync(username, otp, authorizationLevel, now);
            string result = results[0];
            if (result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                result = await CreateCookieAsync(results[1]);
                if (result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    //_logService.CreateLog(DateTime.Now, "Server", username, "Info", "Authentication Succeeded");
                    return new OkResult();
                }
            }
            // {category}: {error message}
            //_logService.CreateLog(DateTime.Now, "Error", username, error[0], error[1]);
            string[] error = result.Split(": ");
            return StatusCode(Convert.ToInt32(error[0]), error[2]);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> CreateCookieAsync(string jwtToken)
        {
            string result;
            try
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.IsEssential = true;
                cookieOptions.Expires = DateTime.Now.AddDays(5);
                cookieOptions.Secure = true;
                Response.Cookies.Append("TresearchAuthenticationCookie", jwtToken, cookieOptions);
                result = _messageBank.SuccessMessages["generic"];
            }catch(Exception ex)
            {
                result = _messageBank.ErrorMessages["cookieFail"];
                /*_logService.CreateLog(DateTime.Now, "Error", _username, "Server", "Authentication Cookie " +
                    "creation failed");*/
            }
            return result;
        }

    }
}
