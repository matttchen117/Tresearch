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
        public string VerifyAccount(IAccount account);
        public List<string> Authenticate(IOTPClaim otpClaim);

        // Authorization
        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredRole);

        // Request OTP
        public string StoreOTP(IOTPClaim otpClaim);

        // Usage Analysis Dashboard
        public List<IKPI> LoadKPI(DateTime now);

        // Delete account
        public string DeleteAccount(IRolePrincipal principal);
    }
}
