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
    // Summary:
    //     A service class for requesting OTPs.
    public class OTPRequestService : IOTPRequestService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        public OTPRequestService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        //
        // Summary:
        //     Verifies the Account and calls the DAO to store an OTPClaim for the corresponding Account
        //
        // Parameters:
        //   account:
        //     The Account to verify and store the OTPClaim for.
        //   otpClaim:
        //     The OTPClaim to store.
        //
        // Returns:
        //     The result of the verification/storing process.
        public string RequestOTP(IAccount account, IOTPClaim otpClaim)
        {
            string result = _sqlDAO.VerifyAccount(account);
            if (result.Equals(_messageBank.SuccessMessages["generic"]))
            {
                result = _sqlDAO.StoreOTP(otpClaim);
            }
            return result;
        }
    }
}