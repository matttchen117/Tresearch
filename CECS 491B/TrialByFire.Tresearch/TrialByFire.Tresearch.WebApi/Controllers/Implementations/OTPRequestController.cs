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
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IOTPRequestManager _otpRequestManager { get; }
        private IMessageBank _messageBank { get; }

        public OTPRequestController(ISqlDAO sqlDAO, ILogService logService, 
            IOTPRequestManager otpRequestManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
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
        //     The selected authorization level for the Account that the User is trying to get an
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
                string[] split;
                string result = await _otpRequestManager.RequestOTPAsync(username, passphrase, 
                    authorizationLevel).ConfigureAwait(false);
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.storeOTPSuccess)
                    .ConfigureAwait(false)))
                {
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                split = result.Split(": ");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch(OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }catch(Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
