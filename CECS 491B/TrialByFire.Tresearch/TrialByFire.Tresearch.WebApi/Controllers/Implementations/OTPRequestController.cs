using Microsoft.AspNetCore.Mvc;
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
        public string RequestOTP(string username, string passphrase, string authorizationLevel)
        {
            string result = _otpRequestManager.RequestOTP(username, passphrase, authorizationLevel);
            if (result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                return result;
            }
            string[] error = result.Split(": ");
            Response.StatusCode = Convert.ToInt32(error[0]);
            return result;
        }
    }
}
