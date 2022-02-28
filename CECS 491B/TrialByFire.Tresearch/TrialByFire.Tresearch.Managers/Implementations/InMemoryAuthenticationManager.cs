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
using TrialByFire.Tresearch.Services.Implentations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class InMemoryAuthenticationManager : IAuthenticationManager
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private readonly IAuthenticationService _authenticationService;
        public InMemoryAuthenticationManager()
        {
            _sqlDAO = new InMemorySqlDAO();
            _logService = new InMemoryLogService(_sqlDAO);
            _authenticationService = new InMemoryAuthenticationService();
        }
        public List<string> Authenticate(string username, string otp, DateTime now)
        {
            IOTPClaim resultClaim = new OTPClaim(username, otp, now);
            List<string> results = _authenticationService.Authenticate(resultClaim);
            return results;
        }
    }
}
