using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SqlAuthenticationManager : IAuthenticationManager
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private readonly IAuthenticationService _authenticationService;
        public SqlAuthenticationManager(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authenticationService = new SqlAuthenticationService(_sqlDAO, _logService);
        }
        public List<string> Authenticate(string username, string otp, DateTime now)
        {
            IOTPClaim resultClaim = new OTPClaim(username, otp, now);
            List<string> results = _authenticationService.Authenticate(resultClaim);
            return results;
        }
    }
}
