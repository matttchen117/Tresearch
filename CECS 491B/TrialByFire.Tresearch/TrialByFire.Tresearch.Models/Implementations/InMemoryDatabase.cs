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
        public IList<IConfirmationLink> ConfirmationLinks { get; set; }
        public IList<IRolePrincipal> RolePrincipals { get; set; }


        public InMemoryDatabase()
        {
            OTPClaims = InitializeOTPClaims();
            Accounts = InitializeAccounts();
            Nodes = InitializeNodes();
            Tags = InitializeTags();
            NodeTags = InitializeNodeTags();
            Ratings = InitializeRatings();
            DailyLogins = InitializeDailyLogins();
            TopSearches = InitializeTopSearches();
            NodesCreated = InitializeNodesCreated();
            ConfirmationLinks = InitializeConfirmationLinks();
            RolePrincipals = InitializeRolePrincipals();
        }

        private List<IOTPClaim> InitializeOTPClaims()
        {
            List<IOTPClaim> otpClaims = new List<IOTPClaim>();
            otpClaims.Add(new OTPClaim("larry@gmail.com", "ABCdef123", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("billy@yahoo.com", "ABCdef123", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("joe@outlook.com", "ABCdef123", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("bob@yahoo.com", "ABCdef123", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("harry@yahoo.com", "ABCdef123", new DateTime(2022, 3, 4, 5, 6, 0)));
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
            return accounts;
        }

        private List<INode> InitializeNodes()
        {
            List<INode> nodes = new List<INode>();
            return nodes;
        }

        private List<ITag> InitializeTags()
        {
            List<ITag> tags = new List<ITag>();
            return tags;
        }

        private List<INodeTag> InitializeNodeTags()
        {
            List<INodeTag> nodeTags = new List<INodeTag>();
            return nodeTags;
        }

        private List<IRating> InitializeRatings()
        {
            List<IRating> ratings = new List<IRating>();
            return ratings;
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

        private List<IConfirmationLink> InitializeConfirmationLinks()
        {
            List<IConfirmationLink> confirmationLinks = new List<IConfirmationLink>();
            return confirmationLinks;
        }

        private List<IRolePrincipal> InitializeRolePrincipals()
        {
            List<IRolePrincipal> rolePrincipals = new List<IRolePrincipal>();
            rolePrincipals.Add(new RolePrincipal(new RoleIdentity(true, "larry@gmail.com", "user")));
            rolePrincipals.Add(new RolePrincipal(new RoleIdentity(true, "billy@yahoo.com", "admin")));
            rolePrincipals.Add(new RolePrincipal(new RoleIdentity(true, "joe@outlook.com", "user")));
            return rolePrincipals;
        }
    }
}
