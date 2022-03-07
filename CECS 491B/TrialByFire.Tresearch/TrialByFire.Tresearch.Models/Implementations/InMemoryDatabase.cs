using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class InMemoryDatabase : IDatabase
    {
        public IList<IOTPClaim> OTPClaims { get; set; }
        public IList<IAccount> Accounts { get; set; }
        public IList<INode> Nodes { get; set; }
        public IList<ITag> Tags { get; set; }
        public IList<INodeTag> NodeTags { get; set; }
        public IList<IRating> Ratings { get; set; }
        /*        public IList<ITreeHistory> TreeHistories { get; set; }
                public IList<IWebPageKPI> WebPageKPIs { get; set; }
                public IList<IDailyRegistrationKPI> DailyRegistrationKPIs { get; set; }*/
        public IList<IDailyLogin> DailyLogins { get; set; }
        public IList<ITopSearch> TopSearches { get; set; }
        public IList<INodesCreated> NodesCreated { get; set; }

        public IList<IDailyRegistration> DailyRegistrations { get; set; }

        public IList<IConfirmationLink> ConfirmationLinks { get; set; }

        public InMemoryDatabase()
        {
            OTPClaims = InitializeOTPClaims();
            Accounts = InitializeAccounts();
            Nodes = InitializeNodes();
            Tags = InitializeTags();
            NodeTags = InitializeNodeTags();
            Ratings = InitializeUserRatings();
            DailyLogins = InitializeDailyLogins();
            TopSearches = InitializeTopSearches();
            NodesCreated = InitializeNodesCreated();
            DailyRegistrations = InitializeDailyRegistrations();
            ConfirmationLinks = InitializeConfirmationLinks();
        }

        /*
            In memory database initialization methods
         */

        private List<IOTPClaim> InitializeOTPClaims()
        {
            List<IOTPClaim> otpClaims = new List<IOTPClaim>();
            otpClaims.Add(new OTPClaim("larry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("billy@yahoo.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("joe@outlook.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("bob@yahoo.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("harry@yahoo.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("barry@yahoo.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0), 4));
            return otpClaims;
        }

        private List<IAccount> InitializeAccounts()
        {
            List<IAccount> accounts = new List<IAccount>();
            accounts.Add(new Account("larry@gmail.com", "larry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("billy@yahoo.com", "billy@yahoo.com", "abcDEF123", "admin", true, true));
            accounts.Add(new Account("joe@outlook.com", "joe@outlook.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("bob@yahoo.com", "bob@yahoo.com", "abcDEF123", "user", false, true));
            accounts.Add(new Account("harry@yahoo.com", "harry@yahoo.com", "abcDEF123", "user", false, false));
            accounts.Add(new Account("barry@yahoo.com", "barry@yahoo.com", "abcDEF123", "user", true, true));

            /*            Viet adding new accounts
                        [InlineData("grizzly@gmail.com", "user", "success")]
                        [InlineData("salewa@gmail.com", "admin", "success")]
                        Accounts for AccountDeletionController*/
            accounts.Add(new Account("grizzly@gmail.com", "grizzly@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new Account("salewa@gmail.com", "salewa@gmail.com", "asdfasdf123", "admin", true, true));

            /*            Accounts for AccountDeletionManager
                        [InlineData("trizip@gmail.com", "user", "success")]
                        [InlineData("switchblade@gmail.com", "admin", "success")]*/
            accounts.Add(new Account("trizip@gmail.com", "trizip@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new Account("switchblade@gmail.com", "switchblade@gmail.com", "asdfasdf123", "admin", true, true));
            /*            Accounts for AccountDeletionService
                        [InlineData("altyn@gmail.com", "user", "success")]
                        [InlineData("ryst@gmail.com", "admin", "success")]*/

            accounts.Add(new Account("altyn@gmail.com", "altyn@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new Account("ryst@gmail.com", "ryst@gmail.com", "asdfasdf123", "admin", true, true));
            return accounts;
        }

        private List<INode> InitializeNodes()
        {
            List<INode> nodes = new List<INode>();
            nodes.Add(new Node(100000, 100001, "Title 1", "Summary 1", "Private", "larry@gmail.com"));
            nodes.Add(new Node(100001, 100002, "Title 2", "Summary 2", "Private", "larry@gmail.com"));
            nodes.Add(new Node(100002, 100003, "Title 3", "Summary 3", "Private", "larry@gmail.com"));
            nodes.Add(new Node(100003, 100004, "Title 4", "Summary 4", "Public", "larry@gmail.com"));
            nodes.Add(new Node(100004, 100004, "Title 5", "Summary 5", "Public", "larry@gmail.com"));

            nodes.Add(new Node(200000, 200001, "Title 1", "Summary 1", "Private", "billy@yahoo.com"));
            nodes.Add(new Node(200001, 200002, "Title 2", "Summary 2", "Private", "billy@yahoo.com"));
            nodes.Add(new Node(200002, 200003, "Title 3", "Summary 3", "Private", "billy@yahoo.com"));
            nodes.Add(new Node(200003, 200004, "Title 4", "Summary 4", "Public", "billy@yahoo.com"));
            nodes.Add(new Node(200004, 200004, "Title 5", "Summary 5", "Public", "billy@yahoo.com"));

            return nodes;
        }

        private List<ITag> InitializeTags()
        {
            List<ITag> tags = new List<ITag>();
            tags.Add(new Tag("Easy"));
            tags.Add(new Tag("Intermediate"));
            tags.Add(new Tag("Hard"));

            return tags;
        }

        private List<INodeTag> InitializeNodeTags()
        {
            List<INodeTag> nodeTags = new List<INodeTag>();
            nodeTags.Add(new NodeTag(100000, "Easy"));
            nodeTags.Add(new NodeTag(100001, "Intermediate"));
            nodeTags.Add(new NodeTag(100002, "Hard"));
            nodeTags.Add(new NodeTag(100003, "Easy"));
            nodeTags.Add(new NodeTag(100004, "Intermediate"));
            nodeTags.Add(new NodeTag(200000, "Easy"));
            nodeTags.Add(new NodeTag(200001, "Intermediate"));
            nodeTags.Add(new NodeTag(200002, "Hard"));
            nodeTags.Add(new NodeTag(200003, "Intermediate"));
            nodeTags.Add(new NodeTag(200004, "Easy"));
            return nodeTags;
        }

        private List<IRating> InitializeUserRatings()
        {
            List<IRating> userRatings = new List<IRating>();
            userRatings.Add(new Rating("larry@gmail.com", 200000, 4));
            userRatings.Add(new Rating("larry@gmail.com", 200001, 5));
            userRatings.Add(new Rating("larry@gmail.com", 200002, 5));
            userRatings.Add(new Rating("larry@gmail.com", 200003, 5));

            userRatings.Add(new Rating("billy@gmail.com", 100000, 4));
            userRatings.Add(new Rating("billy@gmail.com", 100001, 5));
            userRatings.Add(new Rating("billy@gmail.com", 100002, 5));
            userRatings.Add(new Rating("billy@gmail.com", 100003, 5));

            return userRatings;
        }

        private List<IDailyLogin> InitializeDailyLogins()
        {
            List<IDailyLogin> dailyLogins = new List<IDailyLogin>();
            return dailyLogins;
        }

        private List<ITopSearch> InitializeTopSearches()
        {
            List<ITopSearch> topSearches = new List<ITopSearch>();
            return topSearches;
        }

        private List<INodesCreated> InitializeNodesCreated()
        {
            List<INodesCreated> nodesCreated = new List<INodesCreated>();
            return nodesCreated;
        }

        private List<IDailyRegistration> InitializeDailyRegistrations()
        {
            return new List<IDailyRegistration>();
        }

        private List<IConfirmationLink> InitializeConfirmationLinks()
        {
            List<IConfirmationLink> confirmationLinks = new List<IConfirmationLink>();
            return confirmationLinks;
        }
    }
}