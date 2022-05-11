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
    /// <summary>
    ///     OTPRequestManager: Class that is part of the Manager abstraction layer that handles business rules related to OTPRequest
    /// </summary>
    public class OTPRequestManager : IOTPRequestManager
    {
        private IOTPRequestService _otpRequestService { get; }
        private IAccountVerificationService _accountVerificationService { get; }
        private IMessageBank _messageBank { get; }
        private IMailService _mailService { get; }
        private BuildSettingsOptions _options { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // For Testing Only
        //private ILogManager _logManager;

        /// <summary>
        ///     public OTPRequestManager():
        ///         Constructor for OTPRequestManager class
        /// </summary>
        /// <param name="otpRequestService">Service object for Service abstraction layer to perform services related to OTPRequest</param>
        /// <param name="accountVerificationService">Service object for Service abstraction layer to perform services related to AccountVerification</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="mailService">Service object for Service abstraction layer to perform services related to MailService</param>
        /// <param name="options">Snapshot object that represents the setings/configurations of the application</param>
        public OTPRequestManager(IOTPRequestService otpRequestService, 
            IAccountVerificationService accountVerificationService, IMessageBank messageBank, 
            IMailService mailService, IOptionsSnapshot<BuildSettingsOptions> options/*, ILogManager logManager*/)
        {
            _otpRequestService = otpRequestService;
            _accountVerificationService = accountVerificationService;
            _messageBank = messageBank;
            _mailService = mailService;
            _options = options.Value;
            //_logManager = logManager;
        }

        /// <summary>
        ///     RequestOTPAsync:
        ///         Async method that checks business rules related to RequestOTP before calling Service layer
        /// </summary>
        /// <param name="username">The username input by the user</param>
        /// <param name="passphrase">The passphrase input by the user</param>
        /// <param name="authorizationLevel">The authorization level of the operation</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The result of the operation</returns>
        public async Task<string> RequestOTPAsync(string username, string passphrase, 
            string authorizationLevel, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string result;
            // Check if principal not set
            if(Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.principalNotSet).ConfigureAwait(false);
            }
            try
            {
                if(Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // Configure objects and OTP
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
                            // Only send email if not test environment and there is an API key
                            if (!_options.Environment.Equals("Test") && !_options.SendGridAPIKey.Equals(""))
                            {
                                result = await _mailService.SendOTPAsync(account.Username, otp, otp, otp, _cancellationTokenSource.Token).ConfigureAwait(false);
                            }
                            //await _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), level: ILogManager.Levels.Info, category: ILogManager.Categories.Server, otp);
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

        /// <summary>
        ///     GenerateRandomOTPAsync:
        ///         Async method that generates a random OTP based on business rules related to OTPRequest
        /// </summary>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The otp generated</returns>
        private async Task<string> GenerateRandomOTPAsync(CancellationToken cancellationToken = default)
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
