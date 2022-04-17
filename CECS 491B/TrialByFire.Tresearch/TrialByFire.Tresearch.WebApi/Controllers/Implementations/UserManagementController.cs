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
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]
    public class UserManagementController: ControllerBase, IUserManagementController
    {
        // User tree data updated within 5 seconds, as per BRD
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        private ISqlDAO _sqlDAO { get; set; }
        private ILogManager _logManager { get; set; }
        private IMessageBank _messageBank { get; set; }
        private IUserManagementManager _userManagementManager { get; set; }
        private BuildSettingsOptions _options { get; }
        public UserManagementController(ISqlDAO sqlDAO, ILogManager logManager, IMessageBank messageBank, IUserManagementManager userManagementManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logManager = logManager;
            _messageBank = messageBank;
            _userManagementManager = userManagementManager;
            _options = options.Value;
        }

        [HttpGet]
        [Route("createAccount")]
        public async Task<IActionResult> CreateAccountAsync(IAccount account)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _userManagementManager.CreateAccountAsync(account, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "UserManagement: Account Created.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }    
        }

        [HttpGet]
        [Route("updateAccount")]
        public async Task<IActionResult> UpdateAccountAsync(IAccount account, IAccount updatedAccount)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _userManagementManager.UpdateAccountAsync(account, updatedAccount, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountUpdateSuccess)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "UserManagement: Account Created.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }
        }

        [HttpGet]
        [Route("deleteAccount")]
        public async Task<IActionResult> DeleteAccountAsync(IAccount account)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _userManagementManager.DeleteAccountAsync(account, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "UserManagement: Account Created.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }
        }

        [HttpGet]
        [Route("enableAccount")]
        public async Task<IActionResult> EnableAccountAsync(IAccount account)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _userManagementManager.EnableAccountAsync(account, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountEnableSuccess)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "UserManagement: Account Created.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }
        }

        [HttpGet]
        [Route("disableAccount")]
        public async Task<IActionResult> DisableAccountAsync(IAccount account)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _userManagementManager.CreateAccountAsync(account, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountDisableSuccess)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "UserManagement: Account Created.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }
        }

    }
}
