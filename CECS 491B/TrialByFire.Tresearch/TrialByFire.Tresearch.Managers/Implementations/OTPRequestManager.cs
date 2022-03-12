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
    // Summary:
    //     A manager class for enforcing the business rules for requesting an OTP and calling the
    //     appropriate services for the operation.
    public class OTPRequestManager : IOTPRequestManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IValidationService _validationService { get; }
        private IAuthenticationService _authenticationService { get; }

        private IRolePrincipal _rolePrincipal { get; }
        private IOTPRequestService _otpRequestService { get; }
        private IMessageBank _messageBank { get; }

        private IMailService _mailService { get; }

        public OTPRequestManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, 
            IAuthenticationService authenticationService, IRolePrincipal rolePrincipal, 
            IOTPRequestService otpRequestService, IMessageBank messageBank, IMailService mailService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _rolePrincipal = rolePrincipal;
            _otpRequestService = otpRequestService;
            _messageBank = messageBank;
            _mailService = mailService;
        }

        //
        // Summary:
        //     Checks that the User is not currently logged in. Calls the Validation Service to
        //     do basic input validation on the Users inputted username and passphrase. Creates
        //     Account and OTPClaim objects to be passed in to the call to the OTPRequestService.
        //     Emails to OTP to the User.
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
        //     The result of the operation.
        public string RequestOTP(string username, string passphrase, string authorizationLevel)
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
                        IAccount account = new Account(username, passphrase, authorizationLevel);
                        IOTPClaim otpClaim = new OTPClaim(account);
                        result = _otpRequestService.RequestOTP(account, otpClaim);
                        if(result.Equals(_messageBank.SuccessMessages["generic"]))
                        {
                            result = _mailService.SendOTP(account.Username, otpClaim.OTP, otpClaim.OTP, otpClaim.OTP);
                        }
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
