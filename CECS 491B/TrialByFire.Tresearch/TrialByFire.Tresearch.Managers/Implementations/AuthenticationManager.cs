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
    public class AuthenticationManager : IAuthenticationManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IValidationService _validationService { get; }
        private IAuthenticationService _authenticationService { get; }
        private IRolePrincipal _rolePrincipal { get; }
        private IMessageBank _messageBank { get; }

        public AuthenticationManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, 
            IAuthenticationService authenticationService, IRolePrincipal rolePrincipal, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _rolePrincipal = rolePrincipal;
            _messageBank = messageBank;
        }

        public List<string> Authenticate(string username, string otp, DateTime now)
        {
            List<string> results = new List<string>();
            try
            {
                if (_rolePrincipal.IsInRole("guest"))
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("otp", otp);
                    string result = _validationService.ValidateInput(keyValuePairs);
                    if(result.Equals(_messageBank.SuccessMessages["generic"]))
                    {
                        IOTPClaim resultClaim = new OTPClaim(username, otp, now);
                        results = _authenticationService.Authenticate(resultClaim);
                        return results;
                    }
                    results.Add(result);
                    return results;
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
