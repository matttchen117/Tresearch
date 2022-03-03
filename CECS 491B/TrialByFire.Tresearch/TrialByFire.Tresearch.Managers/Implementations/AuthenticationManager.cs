using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
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
        private IPrincipal _rolePrincipal { get; }

        public AuthenticationManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, 
            IAuthenticationService authenticationService, IPrincipal rolePrincipal)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _authenticationService = authenticationService;
            _rolePrincipal = rolePrincipal;
        }

        public List<string> Authenticate(string username, string otp, DateTime now)
        {
            List<string> results = new List<string>();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("username", username);
            keyValuePairs.Add("otp", otp);
            results.Add(_validationService.ValidateInput(keyValuePairs));
            IOTPClaim resultClaim = new OTPClaim(username, otp, now);
            results = _authenticationService.Authenticate(resultClaim);
            return results;
        }
    }
}
