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
        public List<string> CreateAccount(IAccount account);
        public List<string> CreateConfirmationLink(IConfirmationLink _confirmationlink);

        public List<string> ConfirmAccount(IAccount account);

        public List<string> RemoveConfirmationLink(IConfirmationLink confirmationLink);
        public IConfirmationLink GetConfirmationLink(string url);

        public IAccount GetUnconfirmedAccount(string email);

        // Authentication
        public string VerifyAccount(IAccount account);
        public List<string> Authenticate(IOTPClaim otpClaim);

        // Authorization
        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredAuthLevel);

        // Request OTP
        public string StoreOTP(IOTPClaim otpClaim);

        // Usage Analysis Dashboard
        public List<IKPI> LoadKPI(DateTime now);

        // Delete account
        public string DeleteAccount(IRolePrincipal rolePrincipal);

        /*
            Ian's Methods
         */
        
        /*
        public string CreateNode();

        public INode GetNode();

        public string UpdateNode();

        public string DeleteNode();


        public string CreateTag();

        public ITag GetTag();

        public string UpdateTag();

        public string DeleteTag();


        public string CreateNodeTag();

        public INodeTag GetNodeTag();

        public string UpdateNodeTag();

        public string DeleteNodeTag();


        public string CreateRating();

        public IRating GetRating();

        public string UpdateRating();

        public string DeleteRating();

        //*/




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
