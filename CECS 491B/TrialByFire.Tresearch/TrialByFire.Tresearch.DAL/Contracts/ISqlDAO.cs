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
        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim);
        public string StoreOTP(IOTPClaim otpClaim);

        // Usage Analysis Dashboard
        public List<IKPI> LoadKPI(DateTime now);

        // Delete account
        public string DeleteAccount(IRolePrincipal principal);

        /*
            Ian's Methods
         */

        public string CreateNodesCreated(INodesCreated nodesCreated);

        public INodesCreated GetNodesCreated(DateTime nodeCreationDate);

        public string UpdateNodesCreated(INodesCreated nodesCreated);



        public string CreateDailyLogin(IDailyLogin dailyLogin);

        public IDailyLogin GetDailyLogin(DateTime nodeCreationDate);

        public string UpdateDailyLogin(IDailyLogin dailyLogin);


        public string CreateTopSearch(ITopSearch topSearch);

        public ITopSearch GetTopSearch(DateTime nodeCreationDate);

        public string UpdateTopSearch(ITopSearch topSearch);


        public string CreateDailyRegistration(IDailyRegistration dailyRegistration);

        public IDailyRegistration GetDailyRegistration(DateTime nodeCreationDate);

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration);
    }
}
