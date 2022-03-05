using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class OTPRequestService : IOTPRequestService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        public OTPRequestService(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
        }

        public string RequestOTP(IAccount account, IOTPClaim otpClaim)
        {
            string result = _sqlDAO.VerifyAccountEnabled(account);
            if(result.Equals("success"))
            {
                result = _sqlDAO.StoreOTP(otpClaim);
            }
            return result;
        }
    }
}
