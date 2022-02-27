using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IAuthenticationService
    {
        ISqlDAO _sqlDAO { get; set; }
        ILogService _logService { get; set; }

        string _payload { get; set; }

        List<string> Authenticate(IOTPClaim _otpClaim);

        List<string> CreateJwtToken(string _payload);
    }
}
