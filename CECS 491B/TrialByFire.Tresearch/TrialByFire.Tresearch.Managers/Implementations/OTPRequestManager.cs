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
        private IOTPRequestService _otpRequestService { get; }
        private IAccountVerificationService _accountVerificationService { get; }
        private IMessageBank _messageBank { get; }
        private IMailService _mailService { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            /*TimeSpan.FromSeconds(5)*/);

        public OTPRequestManager(ISqlDAO sqlDAO, ILogService logService, IOTPRequestService otpRequestService, 
            IAccountVerificationService accountVerificationService, IMessageBank messageBank, IMailService mailService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _otpRequestService = otpRequestService;
            _accountVerificationService = accountVerificationService;
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
        public async Task<string> RequestOTPAsync(string username, string passphrase, 
            string authorizationLevel, CancellationToken cancellationToken = default)
        {
            // Fires operation cancelled exception
            cancellationToken.ThrowIfCancellationRequested();
            string result;
            try
            {
                if(Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // Basic input validation will be done at client side
                    /*Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("passphrase", passphrase);
                    result = await _validationService.ValidateInputAsync(keyValuePairs);
                    if (result.Equals(_messageBank.SuccessMessages["generic"]))
                    {*/
                    IAccount account = new UserAccount(username, passphrase, authorizationLevel);
                    IOTPClaim otpClaim = new OTPClaim(account);
                    result = await _accountVerificationService.VerifyAccountAsync(account, 
                        _cancellationTokenSource.Token).ConfigureAwait(false);
                    if(result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)
                        .ConfigureAwait(false)))
                    {
                        result = await _otpRequestService.RequestOTPAsync(account, otpClaim,
                        _cancellationTokenSource.Token).ConfigureAwait(false);
                        if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.storeOTPSuccess)
                            .ConfigureAwait(false)))
                        {
                            // No API Key right now
                            result = await _mailService.SendOTPAsync(account.Username, otpClaim.OTP,
                                otpClaim.OTP, otpClaim.OTP, _cancellationTokenSource.Token).ConfigureAwait(false);
                        }
                    }
                    return result;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.alreadyAuthenticated)
                        .ConfigureAwait(false);
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
