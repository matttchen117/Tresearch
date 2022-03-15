using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IAuthenticationManager
    {
        Task<List<string>> AuthenticateAsync(string username, string otp, string authorizationLevel, DateTime now);
    }
}
