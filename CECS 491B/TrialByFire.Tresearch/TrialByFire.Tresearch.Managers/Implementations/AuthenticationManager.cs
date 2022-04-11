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
    // Summary:
    //     A manager class for enforcing the business rules for Authenticating a User and calling the
    //     appropriate services for the operation.
    public class AuthenticationManager : IAuthenticationManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IAccountVerificationService _accountVerificationService { get; }
        private IAuthenticationService _authenticationService { get; }
        private IMessageBank _messageBank { get; }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            /*TimeSpan.FromSeconds(5)*/);

        public AuthenticationManager(ISqlDAO sqlDAO, ILogService logService, 
            IAccountVerificationService accountVerificationService, 
            IAuthenticationService authenticationService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _accountVerificationService = accountVerificationService;
            _authenticationService = authenticationService;
            _messageBank = messageBank;
        }

        //
        // Summary:
        //     
        //
        // Parameters:
        //   username:
        //     The username entered by the User attempting to Authenticate.
        //   otp:
        //     The otp entered by the User attempting to Authenticate.
        //   authorizationLevel:
        //     The selected authorization level for the UserAccount that the User is trying to Authenticate for.
        //
        // Returns:
        //     The result of the operation.
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
                        _cancellationTokenSource.Token)
                        .ConfigureAwait(false);
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

        public async Task<List<string>> RefreshSessionAsync(CancellationToken cancellationToken = default)
        {
            // UserAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, (Thread.CurrentPrincipal.Identity as IRoleIdentity).AuthorizationLevel);
            // Currently leveraging middleware
            // 
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
