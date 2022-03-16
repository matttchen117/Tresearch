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
        private IValidationService _validationService { get; }
        private IAuthenticationService _authenticationService { get; }
        private IMessageBank _messageBank { get; }

        public AuthenticationManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, 
            IAuthenticationService authenticationService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
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
        public async Task<List<string>> AuthenticateAsync(string username, string otp, string authorizationLevel, DateTime now, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            try
            {
                if (Thread.CurrentPrincipal == null)
                {
                    // Basic input validation will be done at client side
 /*                   Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("otp", otp);
                    string result = _validationService.ValidateInput(keyValuePairs);
                    if(result.Equals(_messageBank.SuccessMessages["generic"]))
                    {*/
                    IOTPClaim resultClaim = new OTPClaim(username, otp, authorizationLevel, now);
                    results = await _authenticationService.AuthenticateAsync(resultClaim, cancellationToken).ConfigureAwait(false);
                    return results;
                    /*}
                    results.Add(result);
                    return results;*/
                }
                results.Add(_messageBank.ErrorMessages["alreadyAuthenticated"]);
            }catch(OTPClaimCreationFailedException occfe)
            {
                results.Add(occfe.Message);
            }
            return results;
        }
    }
}
