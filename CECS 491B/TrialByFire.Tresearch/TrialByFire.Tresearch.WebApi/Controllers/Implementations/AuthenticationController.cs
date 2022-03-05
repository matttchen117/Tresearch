using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
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

        // IEnumerable may be faster than using lists, gives compiler chance to defer work to later, possibly optimizing in the process
        public string Authenticate(string username, string otp)
        {
            _username = username;
            List<string> results = _authenticationManager.Authenticate(username, otp, DateTime.Now);
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

        public string Authenticate(string username, string otp, DateTime now)
        {
            _username = username;
            List<string> results = _authenticationManager.Authenticate(username, otp, now);
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

        [HttpPost]
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
                result = "success";
            }catch(Exception ex)
            {
                result = "Server: Authentication Cookie creation failed";
                /*_logService.CreateLog(DateTime.Now, "Error", _username, "Server", "Authentication Cookie " +
                    "creation failed");*/
            }
            return result;
        }

    }
}
