using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    public interface ISqlDAO
    {
        public Task<string> StoreLogAsync(ILog log, CancellationToken cancellationToken = default);
        public Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken= default(CancellationToken));
        public Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateConfirmationLinkAsync(IConfirmationLink _confirmationlink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountToUnconfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountToConfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));

        // Authentication
        public Task<string> VerifyAccountAsync(IAccount account, CancellationToken cancellationToken = default);
        public Task<List<string>> AuthenticateAsync(IOTPClaim otpClaim, CancellationToken cancellationToken = default);

        // Authorization
        public Task<string> VerifyAuthorizedAsync(string requiredAuthLevel, CancellationToken cancellation);

        // Request OTP
        public Task<string> StoreOTPAsync(IOTPClaim otpClaim, CancellationToken cancellationToken = default);

        // Usage Analysis Dashboard
        //public List<IKPI> LoadKPI(DateTime now);

        // Delete account
        public string DeleteAccount();

        // KPI Methods
        public Task<IViewKPI> GetViewKPIAsync(CancellationToken cancellationToken = default);
        public Task<IViewDurationKPI> GetViewDurationKPIAsync(CancellationToken cancellationToken = default);
        public Task<ILoginKPI> GetLoginKPIAsync(DateTime now, CancellationToken cancellationToken = default);
        public Task<INodeKPI> GetNodeKPIAsync(DateTime now, CancellationToken cancellationToken = default);
        public Task<IRegistrationKPI> GetRegistrationKPIAsync(DateTime now, CancellationToken cancellationToken = default);
        public Task<ISearchKPI> GetSearchKPIAsync(DateTime now, CancellationToken cancellationToken = default);


        public string CreateView(IView view);
        public Task<List<View>> GetAllViewsAsync(CancellationToken cancellationToken = default);


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

        public Task<List<NodesCreated>> GetNodesCreatedAsync(DateTime nodeCreationDate, CancellationToken cancellationToken = default);

        public string UpdateNodesCreated(INodesCreated nodesCreated);



        public string CreateDailyLogin(IDailyLogin dailyLogin);

        public Task<List<DailyLogin>> GetDailyLoginAsync(DateTime nodeCreationDate, CancellationToken cancellationToken = default);

        public string UpdateDailyLogin(IDailyLogin dailyLogin);


        public string CreateTopSearch(ITopSearch topSearch);

        public Task<List<TopSearch>> GetTopSearchAsync(DateTime nodeCreationDate, CancellationToken cancellationToken = default);

        public string UpdateTopSearch(ITopSearch topSearch);


        public string CreateDailyRegistration(IDailyRegistration dailyRegistration);

        public Task<List<DailyRegistration>> GetDailyRegistrationAsync(DateTime nodeCreationDate, CancellationToken cancellationToken = default);

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration);

        public Task<string> AddTagToNodesAsync(List<string> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagFromNodeAsync(List<string> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<string> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
