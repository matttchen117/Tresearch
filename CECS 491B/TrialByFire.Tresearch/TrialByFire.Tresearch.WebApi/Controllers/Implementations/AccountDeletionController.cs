
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]
    public class AccountDeletionController : ControllerBase, IAccountDeletionController
    {


        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; }
        private ILogManager _logManager { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        private IAccountDeletionManager _accountDeletionManager { get; }

        public AccountDeletionController(ISqlDAO sqlDAO, ILogService logService, ILogManager logManager, IMessageBank messageBank, IAccountDeletionManager accountDeletionManager, IOptionsSnapshot<BuildSettingsOptions>  options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _logManager = logManager;
            _messageBank = messageBank;
            _accountDeletionManager = accountDeletionManager;
            _options = options.Value;
        }

        /// <summary>
        /// entry point for Account Deletion Requests
        /// </summary>
        /// <returns>Message stating if deletion succeeded or error during deletion</returns>

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            try
            {
                string[] split;
                string result = "";

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                    role = _options.User;
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                    role = _options.Admin;

                result = await _accountDeletionManager.DeleteAccountAsync(CancellationTokenSource.Token).ConfigureAwait(false);
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false)))
                {
                    if (_options.Environment.Equals("Test"))
                    {
                        split = result.Split(": ");
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                        _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), "Server", Thread.CurrentPrincipal.Identity.Name, role, "Info" , "Account Deletion Success" );
                    }
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), "Server", Thread.CurrentPrincipal.Identity.Name, role, "Info", "Account Deletion Success");

                }
                split = result.Split(": ");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), "Error", "unknownUser", "unknown", split[0], split[2]);
            }
            catch (OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

    }
}
