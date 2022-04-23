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
        private ILogManager _logManager { get; }
        private IMessageBank _messageBank { get; }

        private ILogoutManager _logoutManager { get; }

        private BuildSettingsOptions _options { get; }
        public LogoutController(ISqlDAO sqlDAO, ILogManager logManager, IMessageBank messageBank, 
            ILogoutManager logoutManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logManager = logManager;
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
            try
            {
                string result = await _logoutManager.LogoutAsync().ConfigureAwait(false);
                string[] split = result.Split(": ");
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess)
                    .ConfigureAwait(false)))
                {
                    if (_options.Environment.Equals("Test"))
                    {
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }
                    HttpContext.User = null;
                    Response.Headers.Remove(_options.JWTHeaderName);
                    _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server, 
                        await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess).ConfigureAwait(false));
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
