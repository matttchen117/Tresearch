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
    public class InMemoryOTPRequestService : IOTPRequestService
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;

        public InMemoryOTPRequestService(ISqlDAO inMemorySqlDAO, ILogService inMemoryLogService)
        {
            _sqlDAO = inMemorySqlDAO;
            _logService = inMemoryLogService;
        }

        public string RequestOTP(IAccount account)
        {
            string result = _sqlDAO.VerifyAccount(account);
            IOTPClaim otpClaim = new OTPClaim(account);
            result = _sqlDAO.StoreOTP(otpClaim);
            return result;
        }
    }
}
