using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private IAuthenticationManager _authenticationManager;

        public AuthenticationController(ISqlDAO sqlDAO, ILogService logService, IAuthenticationManager authenticationManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authenticationManager = authenticationManager;
        }

        // IEnumerable may be faster than using lists, gives compiler chance to defer work to later, possibly optimizing in the process
        [HttpPost]
        public string Authenticate(string username, string otp)
        {
            List<string> results = _authenticationManager.Authenticate(username, otp, DateTime.Now);
            string result = results[0];
            if(result.Equals("success"))
            {
                result = CreateCookie(results[1]);
                if(result.Equals("success"))
                {
                    _logService.CreateLog(DateTime.Now, "Server", username, "Info", "Authentication Succeeded");
                    return result;
                }
            }
            // {category}: {error message}
            string[] error = result.Split(": ");
            _logService.CreateLog(DateTime.Now, error[0], username, "Error", error[1]);
            return result;
        }

        public string CreateCookie(string jwtToken)
        {
            // unsure of what errors could actually occur here
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.IsEssential = true;
            cookieOptions.Expires = DateTime.Now.AddDays(5);
            cookieOptions.Secure = true;
            Response.Cookies.Append("AuthN", jwtToken, cookieOptions);
            return "success";
        }

    }
}
