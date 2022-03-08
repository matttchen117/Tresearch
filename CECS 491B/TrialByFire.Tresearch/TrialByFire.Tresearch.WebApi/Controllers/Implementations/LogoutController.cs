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
        public string Logout()
        {
            string result = _logoutManager.Logout();
            if(result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                try
                {
                    Response.Cookies.Delete("AuthN");
                }catch(Exception e)
                {
                    return _messageBank.ErrorMessages["logoutFail"];
                }
            }
            return result;
        }
    }
}
