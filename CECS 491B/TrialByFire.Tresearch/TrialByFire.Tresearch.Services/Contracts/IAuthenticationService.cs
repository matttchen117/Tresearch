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
        List<string> Authenticate(IOTPClaim _otpClaim);

        List<string> CreateJwtToken(string _payload);
    }
}
