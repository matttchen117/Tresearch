using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    // Summary:
    //     A controller class for logging the User out.
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class LogoutController : Controller, ILogoutController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }

        private ILogoutManager _logoutManager { get; }

        private BuildSettingsOptions _options { get; }
        public LogoutController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, 
            ILogoutManager logoutManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _logoutManager = logoutManager;
            _options = options.Value;
        }

        //
        // Summary:
        //     Entry point for Logout requests and deletes the Users Cookie
        //
        // Returns:
        //     The result of the operation with any status codes if applicable.
        [HttpPost]
        [Route("logout")]
        // Do Async if want to log something for DB (last time logged in)
        // Only Async if require operation to be done
        public async Task<IActionResult> LogoutAsync()
        {
            // Check if the cookie is here at this point
            // Middleware is included in everywhere async, therefore everywhere should be async 

            string[] split;
            string result = "";
            try
            {
                //result = await _logoutManager.LogoutAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                result = await _logoutManager.LogoutAsync().ConfigureAwait(false);
                if (result.Equals(_messageBank.GetMessage(IMessageBank.Responses.logoutSuccess)))
                {
                    if(_options.Environment.Equals("Test"))
                    {
                        split = result.Split(": ");
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }
                    HttpContext.User = null;
                    Response.Headers.Remove(_options.JWTHeaderName);
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
            }
            catch (OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex){
                return StatusCode(400, ex.Message);
            }
            split = result.Split(": ");
            return StatusCode(Convert.ToInt32(split[0]), split[2]);
        }
    }
}
