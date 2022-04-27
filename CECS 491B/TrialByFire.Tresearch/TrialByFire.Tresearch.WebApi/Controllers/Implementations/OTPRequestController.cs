using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{

    // Summary:
    //     A controller class for requesting OTPs.
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class OTPRequestController : Controller, IOTPRequestController
    {
        private ILogManager _logManager { get; }
        private IOTPRequestManager _otpRequestManager { get; }
        private IMessageBank _messageBank { get; }

        public OTPRequestController(ILogManager logManager, 
            IOTPRequestManager otpRequestManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _otpRequestManager = otpRequestManager;
            _messageBank = messageBank;
        }

        //
        // Summary:
        //     Entry point for OTP requests and forwards the User input to the RequestManager to
        //     perform the operation.
        //
        // Parameters:
        //   username:
        //     The username entered by the User requesting the OTP.
        //   passphrase:
        //     The passphrase entered by the User requesting the OTP.
        //   authorizationLevel:
        //     The selected authorization level for the UserAccount that the User is trying to get an
        //     OTP for.
        //
        // Returns:
        //     The result of the operation with any status codes if applicable.
        [HttpPost]
        [Route("requestotp")]
        public async Task<IActionResult> RequestOTPAsync(string username, string passphrase, string authorizationLevel)
        {
            try
            {
                string result = await _otpRequestManager.RequestOTPAsync(username, passphrase, 
                    authorizationLevel).ConfigureAwait(false);
                string[] split = result.Split(": ");
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.storeOTPSuccess)
                    .ConfigureAwait(false)))
                {
                    await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                        category: ILogManager.Categories.Server,
                        await _messageBank.GetMessage(IMessageBank.Responses.storeOTPSuccess).ConfigureAwait(false));
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                if (Enum.TryParse(split[1], out ILogManager.Categories category))
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category, split[2]);
                }
                else
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, split[2] + ": Bad category passed back.");
                }
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (OperationCanceledException tce)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + tce.Message);
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level:
                    ILogManager.Levels.Error,
                    category: ILogManager.Categories.Server, await
                    _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false)
                    + ex.Message);
                return StatusCode(600, ex.Message);
            }
        }
    }
}
