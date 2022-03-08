using Microsoft.AspNetCore.Mvc;
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
        [Route("authenticate")]
        public string Authenticate(string username, string otp, string authorizationLevel)
        {
            _username = username;
            List<string> results = _authenticationManager.Authenticate(username, otp, authorizationLevel, DateTime.Now);
            string result = results[0];
            if(result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                result = CreateCookie(results[1]);
                if(result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    //_logService.CreateLog(DateTime.Now, "Server", username, "Info", "Authentication Succeeded");
                    return result;
                }
            }
            // {category}: {error message}
            string[] error = result.Split(": ");
            //_logService.CreateLog(DateTime.Now, "Error", username, error[0], error[1]);
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string Authenticate(string username, string otp, string authorizationLevel, DateTime now)
        {
            _username = username;
            List<string> results = _authenticationManager.Authenticate(username, otp, authorizationLevel, now);
            string result = results[0];
            if (result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                result = CreateCookie(results[1]);
                if (result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    //_logService.CreateLog(DateTime.Now, "Server", username, "Info", "Authentication Succeeded");
                    return result;
                }
            }
            // {category}: {error message}
            string[] error = result.Split(": ");
            //_logService.CreateLog(DateTime.Now, "Error", username, error[0], error[1]);
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string CreateCookie(string jwtToken)
        {
            string result;
            try
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.IsEssential = true;
                cookieOptions.Expires = DateTime.Now.AddDays(5);
                cookieOptions.Secure = true;
                Response.Cookies.Append("AuthN", jwtToken, cookieOptions);
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
