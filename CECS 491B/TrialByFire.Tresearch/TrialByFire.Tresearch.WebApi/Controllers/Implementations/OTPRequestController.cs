using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class OTPRequestController : Controller, IOTPRequestController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IOTPRequestManager _otpRequestManager { get; }
        public OTPRequestController(ISqlDAO sqlDAO, ILogService logService, IOTPRequestManager otpRequestManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _otpRequestManager = otpRequestManager;
        }

        public string RequestOTP(string username, string passphrase, string authorizationLevel)
        {
            return _otpRequestManager.RequestOTP(username, passphrase, authorizationLevel);
        }
    }
}
