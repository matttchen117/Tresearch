using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
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
        private IOTPRequestService _otpRequestService { get; }
        private IAccountVerificationService _accountVerificationService { get; }
        private IMessageBank _messageBank { get; }
        private IMailService _mailService { get; }
        private BuildSettingsOptions _options { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            /*TimeSpan.FromSeconds(5)*/);

        // For Testing Only
        private ILogManager _logManager;

        public OTPRequestManager(IOTPRequestService otpRequestService, 
            IAccountVerificationService accountVerificationService, IMessageBank messageBank, 
            IMailService mailService, IOptionsSnapshot<BuildSettingsOptions> options, ILogManager logManager)
        {
            _otpRequestService = otpRequestService;
            _accountVerificationService = accountVerificationService;
            _messageBank = messageBank;
            _mailService = mailService;
            _options = options.Value;
            _logManager = logManager;
        }

        //
        // Summary:
        //     Checks that the User is not currently logged in. Calls the Validation Service to
        //     do basic input validation on the Users inputted username and passphrase. Creates
        //     UserAccount and OTPClaim objects to be passed in to the call to the OTPRequestService.
        //     Emails to OTP to the User.
        //
        // Parameters:
        //   username:
        //     The username entered by the User requesting the OTP.
        //   passphrase:
        //     The passphrase entered by the User requesting the OTP.
        //   authorizationLevel:
        //     The selected authorization level for the UserAccount that the User is trying to get an
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
                    IAccount account = new UserAccount(username, passphrase, authorizationLevel);
                    string otp = await GenerateRandomOTPAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                    byte[] salt = new byte[0];
                    byte[] key = KeyDerivation.Pbkdf2(otp, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
                    string hash = Convert.ToHexString(key);
                    IOTPClaim otpClaim = new OTPClaim(account, hash);
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
                            if (!_options.Environment.Equals("Test"))
                            {
                                /*result = await _mailService.SendOTPAsync(account.Username, otp,
                                    otp, otp, _cancellationTokenSource.Token).ConfigureAwait(false);*/
                                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info,
                                 category: ILogManager.Categories.Server, otp);
                            }
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

        public async Task<string> GenerateRandomOTPAsync(CancellationToken cancellationToken = default)
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            int length = 8;
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += validCharacters[random.Next(0, validCharacters.Length)];
            }
            return otp;
        }
    }
}
