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
            // for otp request tests
            otpClaims.Add(new OTPClaim("aarry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("barry@gmail.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("carry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("darry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("earry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("farry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0), 4));
            // for authentication tests
            otpClaims.Add(new OTPClaim("garry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("harry@gmail.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("iarry@gmail.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("jarry@gmail.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("karry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("larry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("marry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("narry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0), 4));
            otpClaims.Add(new OTPClaim("oarry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0), 4));
            otpClaims.Add(new OTPClaim("parry@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0), 4));
            return otpClaims;
        }

        private List<IAccount> InitializeAccounts()
        {
            List<IAccount> accounts = new List<IAccount>();
            // for otp request tests
            accounts.Add(new Account("aarry@gmail.com", "aarry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("barry@gmail.com", "barry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new Account("carry@gmail.com", "carry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("darry@gmail.com", "darry@gmail.com", "abcDEF123", "user", false, true));
            accounts.Add(new Account("earry@gmail.com", "earry@gmail.com", "abcDEF123", "user", false, false));
            accounts.Add(new Account("farry@gmail.com", "farry@gmail.com", "abcDEF123", "user", true, true));
            // for authentication tests
            accounts.Add(new Account("garry@gmail.com", "garry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("harry@gmail.com", "harry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new Account("iarry@gmail.com", "iarry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new Account("jarry@gmail.com", "jarry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new Account("karry@gmail.com", "karry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("larry@gmail.com", "larry@gmail.com", "abcDEF123", "user", false, true));
            accounts.Add(new Account("marry@gmail.com", "marry@gmail.com", "abcDEF123", "user", false, false));
            accounts.Add(new Account("narry@gmail.com", "narry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("oarry@gmail.com", "oarry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new Account("parry@gmail.com", "parry@gmail.com", "abcDEF123", "user", true, true));
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