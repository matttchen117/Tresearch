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
            TimeSpan.FromSeconds(5));

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
        //     The selected authorization level for the Account that the User is trying to Authenticate for.
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
                if (Thread.CurrentPrincipal == null)
                {
                    IAccount account = new Account(username, authorizationLevel);
                    IOTPClaim resultClaim = new OTPClaim(username, otp, authorizationLevel, now);
                    string result = await _accountVerificationService.VerifyAccountAsync(account, 
                        _cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses
                        .verifySuccess).ConfigureAwait(false)))
                    {
                        results = await _authenticationService.AuthenticateAsync(resultClaim, 
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
    }
}
