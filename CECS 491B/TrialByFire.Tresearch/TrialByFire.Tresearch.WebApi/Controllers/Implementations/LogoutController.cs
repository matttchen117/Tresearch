using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : Controller, ILogoutController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }

        private ILogoutManager _logoutManager { get; }

        private IRolePrincipal _rolePrincipal { get; }


        public LogoutController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, 
            ILogoutManager logoutManager, IRolePrincipal rolePrincipal)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _logoutManager = logoutManager;
            _rolePrincipal = rolePrincipal;
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            string result = _logoutManager.Logout();
            if(result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                try
                {
                    Response.Cookies.Delete("TresearchAuthenticationCookie");
                    return new OkResult();
                }
                catch(Exception e)
                {
                    result = _messageBank.ErrorMessages["logoutFail"];
                }
            }
            string[] error = result.Split(": ");
            return StatusCode(Convert.ToInt32(error[0]), error[2]);
        }
    }
}
