using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class OTPRequestManager : IOtpRequestManager
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private readonly IOTPRequestService _otpRequestService;

        public OTPRequestManager(ISqlDAO sqlDAO, ILogService logService, IOTPRequestService otpRequestService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _otpRequestService = otpRequestService;
        }
        public string RequestOTP(string username, string passphrase)
        {
            IAccount account = new Account(username, passphrase);
            string result = _otpRequestService.RequestOTP(account);
            return result;
        }
    }
}
