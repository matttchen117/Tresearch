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
        public Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput); 
        public Task<string> RemoveUserIdentityFromHashTable(string email, string authorizationLevel, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateUserHashAsync(int ID, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> GetUserHashAsync(IAccount account, CancellationToken cancellationToken = default);
        public Task<int> StoreLogAsync(ILog log, string destination, CancellationToken cancellationToken = default);
        public Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken= default(CancellationToken));
        public Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateOTPAsync(string username, string authorizationLevel, int failCount, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<int, string>> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateConfirmationLinkAsync(IConfirmationLink _confirmationlink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountToUnconfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> UpdateAccountToConfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> IsAuthorizedToMakeNodeChangesAsync(List<long> nodeIDs, string userHash, CancellationToken cancellationToken = default(CancellationToken));

        // Authentication
        public Task<int> VerifyAccountAsync(IAccount account, CancellationToken cancellationToken = default);
        public Task<int> AuthenticateAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default);

        // Request OTP
        public Task<int> StoreOTPAsync(IAccount account, IOTPClaim otpClaim, CancellationToken cancellationToken = default);

        // Usage Analysis Dashboard
        //public List<IKPI> LoadKPI(DateTime now);

        // Delete account
        public Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        // Get admins
        public Task<string> GetAmountOfAdminsAsync(CancellationToken cancellationToken = default(CancellationToken));


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

        //Rating
        public Task<string> RateNodeAsync(string userhash, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<double>, string>> GetNodeRatingAsync(List<long> nodeID, CancellationToken cancellationToken = default(CancellationToken));


        /**
         *  User Management
         *      Create Account
         *      Update Account
         *      Delete Account
         *      Disable Account
         *      Enable Account
         */

        public Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken));

        

        public Task<string> CreateNodeAsync(INode node, CancellationToken cancellationToken = default);
        public Task<string> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default);
        public Task<string> UpdateNodeAsync(INode updatedNode, INode previousNode, CancellationToken cancellationToken = default);
        public Task<Tuple<INode, string>> GetNodeAsync(long nID, CancellationToken cancellationToken = default);
        public Task<Tuple<List<INode>, string>> GetNodesAsync(string userHash, string acccountHash, CancellationToken cancellationToken = default);
        public Task<Tuple<List<INode>, string>> GetNodeChildren(long nID, CancellationToken cancellationToken = default);
        
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

        /// <summary>
        ///     Adds tag to list of node(s)
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public Task<string> AddTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Removes tag from list of node(s)
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public Task<string> RemoveTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Retrieves list of shared tags from a list of node(s). List with single node will return all tags.
        /// </summary>
        /// <param name="nodeIDs">List of node ID(s)</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status result</returns>
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Creates a tag in tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="count">Number of nodes tagged</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public Task<string> CreateTagAsync(string tagName, int count, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Deletes a tag from tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Retrieves list of tags from tag bank
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status result</returns>
        public Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));






        public Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken));


        public Task<IResponse<string>> PasteNodeAsync(string userHash, long nodeIDPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> IsNodeLeaf(long nodeIDToPasteTo, CancellationToken cancellationToken = default(CancellationToken));

    }
}
