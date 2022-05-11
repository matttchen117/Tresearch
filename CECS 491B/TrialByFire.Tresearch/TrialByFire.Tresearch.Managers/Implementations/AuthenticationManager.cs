using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    /// <summary>
    ///     AuthenticationManager: Class that is part of the Manager abstraction layer that handles business rules related to Authentication
    /// </summary>
    public class AuthenticationManager : IAuthenticationManager
    {
        private IAccountVerificationService _accountVerificationService { get; }
        private IAuthenticationService _authenticationService { get; }
        private IMessageBank _messageBank { get; }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));

        /// <summary>
        ///     public AuthenticationManager():
        ///         Constructor for AuthenticationManager class
        /// </summary>
        /// <param name="accountVerificationService">Service object for Service abstraction layer to perform services related to AccountVerification</param>
        /// <param name="authenticationService">Service object for Service abstraction layer to perform services related to Authentication</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public AuthenticationManager(IAccountVerificationService accountVerificationService, 
            IAuthenticationService authenticationService, IMessageBank messageBank)
        {
            _accountVerificationService = accountVerificationService;
            _authenticationService = authenticationService;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     AuthenticateAsync:
        ///         Async method that checks business rules related to Authentication before calling Service layer
        /// </summary>
        /// <param name="username">The username input by the user</param>
        /// <param name="otp">The otp input by the user</param>
        /// <param name="authorizationLevel">The authorization level for the operation</param>
        /// <param name="now">The time of the operation launch</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The results of the operation</returns>
        public async Task<List<string>> AuthenticateAsync(string username, string otp, 
            string authorizationLevel, DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            try
            {
                if (Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IAccount account = new UserAccount(username, authorizationLevel);
                    IOTPClaim resultClaim = new OTPClaim(username, otp, authorizationLevel, now);

                    string result = await _accountVerificationService.VerifyAccountAsync(account, 
                        _cancellationTokenSource.Token).ConfigureAwait(false);

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses
                        .verifySuccess).ConfigureAwait(false)))
                    {
                        IAuthenticationInput authenticationInput = new AuthenticationInput(account, resultClaim);
                        results = await _authenticationService.AuthenticateAsync(authenticationInput, 
                            _cancellationTokenSource.Token).ConfigureAwait(false);
                    }
                    else
                    {
                        results.Add(result);
                    }
                    return results;
                }
                results.Add(await _messageBank.GetMessage(IMessageBank.Responses.alreadyAuthenticated)
                    .ConfigureAwait(false));
            }catch(OTPClaimCreationFailedException occfe)
            {
                results.Add(occfe.Message);
            }
            return results;
        }

        /// <summary>
        ///     RefreshSessionAsync:
        ///         Async method that checks business rules related to RefreshSession before calling Service layer
        /// </summary>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The results of the operation</returns>
        public async Task<List<string>> RefreshSessionAsync(CancellationToken cancellationToken = default)
        {
            List<string> results = new List<string>();
            try
            {
                if(!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name,
                        (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel);
                    IAuthenticationInput authenticationInput = new AuthenticationInput(account,
                        (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash);

                    return await _authenticationService.RefreshSessionAsync(authenticationInput, _cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                }
                else
                {
                    results.Add(await _messageBank.GetMessage(IMessageBank.Responses.refreshSessionNotAllowed)
                        .ConfigureAwait(false));
                    return results;
                }
            }catch(Exception ex)
            {
                results.Add(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException)
                    .ConfigureAwait(false) + ex.Message);
                return results;
            }

        }
    }
}
