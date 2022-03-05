using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    public interface ISqlDAO
    {
        public bool CreateAccount(IAccount account);
        public bool CreateConfirmationLink(IConfirmationLink _confirmationlink);

        public IConfirmationLink GetConfirmationLink(string url);

        // Authentication
        public string VerifyAccountEnabled(IAccount account);
        public List<string> Authenticate(IOTPClaim otpClaim);
        public string VerifyAuthenticated(IRolePrincipal rolePrincipal);

        // Authorization
        public string Authorize(IRolePrincipal rolePrincipal, string requiredRole);

        // Request OTP
        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim);
        public string StoreOTP(IOTPClaim otpClaim);

        // Usage Analysis Dashboard
        public List<IKPI> LoadKPI(DateTime now);
    }
}
