using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
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

        private IRolePrincipal _rolePrincipal { get; }
        private IOTPRequestService _otpRequestService { get; }
        private IMessageBank _messageBank { get; }

        public OTPRequestManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, 
            IAuthenticationService authenticationService, IRolePrincipal rolePrincipal, 
            IOTPRequestService otpRequestService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _rolePrincipal = rolePrincipal;
            _otpRequestService = otpRequestService;
            _messageBank = messageBank;
        }

        public string RequestOTP(string username, string passphrase)
        {
            string result;
            try
            {
                if(_rolePrincipal.IsInRole("guest"))
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("passphrase", passphrase);
                    result = _validationService.ValidateInput(keyValuePairs);
                    if (result.Equals(_messageBank.SuccessMessages["generic"]))
                    {
                        IAccount account = new Account(username, passphrase);
                        IOTPClaim otpClaim = new OTPClaim(account);
                        result = _otpRequestService.RequestOTP(account, otpClaim);
                    }
                    return result;
                }
                else
                {
                    return _messageBank.ErrorMessages["alreadyAuthenticated"];
                }
            }catch(AccountCreationFailedException acfe)
            {
                return acfe.Message;
            }catch(OTPClaimCreationFailedException occfe)
            {
                return occfe.Message;
            }
        }
    }
}
