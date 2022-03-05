using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class OTPRequestManager : IOTPRequestManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IValidationService _validationService { get; }
        private IAuthenticationService _authenticationService { get; }

        private IPrincipal _rolePrincipal { get; }
        private IOTPRequestService _otpRequestService { get; }

        public OTPRequestManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, IAuthenticationService authenticationService, IPrincipal rolePrincipal, IOTPRequestService otpRequestService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _rolePrincipal = rolePrincipal;
            _otpRequestService = otpRequestService;
        }

        public string RequestOTP(string username, string passphrase)
        {
            string result;
            result = _authenticationService.VerifyNotAuthenticated(_rolePrincipal);
            if(result.Equals("success"))
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("username", username);
                keyValuePairs.Add("passphrase", passphrase);
                result = _validationService.ValidateInput(keyValuePairs);
                if(result.Equals("success"))
                {
                    result = _authenticationService.VerifyAuthenticated(_rolePrincipal);
                    if(result.Equals("success"))
                    {
                        IAccount account = new Account(username, passphrase);
                        IOTPClaim otpClaim = new OTPClaim(account);
                        result = _otpRequestService.RequestOTP(account, otpClaim);
                    }
                }
            }
            return result;
        }
    }
}
