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
        public IList<IView> Views { get; set; }

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
            Views = InitializeViews();
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
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 3)), 100));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 4)), 99));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 5)), 98));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 6)), 97));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 7)), 96));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 8)), 95));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 9)), 94));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 10)), 93));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 11)), 92));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 12)), 91));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 13)), 90));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 14)), 89));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 15)), 88));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 16)), 87));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 17)), 86));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 18)), 85));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 19)), 84));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 20)), 83));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 21)), 82));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 22)), 81));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 23)), 80));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 24)), 79));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 25)), 78));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 26)), 77));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 27)), 76));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 28)), 75));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 29)), 74));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 30)), 73));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 12, 31)), 72));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 1)), 71));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 2)), 70));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 3)), 69));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 4)), 68));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 5)), 67));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 6)), 66));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 7)), 65));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 8)), 64));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 9)), 63));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 10)), 62));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 11)), 61));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 12)), 60));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 13)), 59));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 14)), 58));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 15)), 57));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 16)), 56));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 17)), 55));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 18)), 54));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 19)), 53));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 20)), 52));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 21)), 51));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 22)), 50));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 23)), 49));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 24)), 48));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 25)), 47));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 26)), 46));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 27)), 45));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 28)), 44));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 29)), 43));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 30)), 42));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 1, 31)), 41));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 7)), 40));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 8)), 39));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 9)), 38));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 10)), 37));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 11)), 36));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 12)), 35));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 13)), 34));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 14)), 33));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 15)), 32));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 16)), 31));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 17)), 30));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 18)), 29));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 19)), 28));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 20)), 27));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 21)), 26));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 22)), 25));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 23)), 24));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 24)), 23));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 25)), 22));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 26)), 21));

            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 27)), 20));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 26)), 19));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 27)), 18));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 2, 28)), 17));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 1)), 16));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 2)), 15));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 3)), 14));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 4)), 13));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 5)), 12));
            dailyLogins.Add(new DailyLogin((new DateTime(2022, 3, 6)), 11));
            return dailyLogins;
        }

        private List<ITopSearch> InitializeTopSearches()
        {
            List<ITopSearch> topSearches = new List<ITopSearch>();
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 5)), "Donuts", 20));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 6)), "Dark", 21));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 7)), "Dumb", 22));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 8)), "Dusk", 23));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 9)), "Dot", 24));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 10)), "Dawn", 25));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 11)), "Do", 26));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 12)), "Dont", 27));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 13)), "Dew", 28));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 14)), "Apple", 29));

            topSearches.Add(new TopSearch((new DateTime(2022, 2, 15)), "Answer", 30));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 16)), "Always", 31));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 17)), "Almost", 32));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 18)), "Accept", 33));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 19)), "Adapt", 34));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 20)), "Overcome", 35));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 21)), "Outside", 36));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 22)), "Never", 37));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 23)), "Dope", 38));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 24)), "Pizza", 39));

            topSearches.Add(new TopSearch((new DateTime(2022, 2, 25)), "Part", 40));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 26)), "Xbox", 41));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 27)), "Playstation", 42));
            topSearches.Add(new TopSearch((new DateTime(2022, 2, 28)), "Switch", 43));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 1)), "Cats", 46));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 2)), "Turtles", 47));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 3)), "Hamsters", 48));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 4)), "Fish", 49));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 5)), "Cows", 45));
            topSearches.Add(new TopSearch((new DateTime(2022, 3, 6)), "Dogs", 50));
            return topSearches;
        }

        private List<INodesCreated> InitializeNodesCreated()
        {
            List<INodesCreated> nodesCreated = new List<INodesCreated>();
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 5)), 70));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 6)), 71));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 7)), 72));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 8)), 73));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 9)), 74));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 10)), 75));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 11)), 76));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 12)), 77));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 13)), 78));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 14)), 79));

            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 15)), 80));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 16)), 81));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 17)), 82));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 18)), 83));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 19)), 84));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 20)), 85));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 21)), 86));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 22)), 87));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 23)), 88));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 24)), 89));

            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 25)), 90));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 26)), 91));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 27)), 92));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 2, 28)), 93));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 1)), 94));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 2)), 96));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 3)), 97));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 4)), 98));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 5)), 99));
            nodesCreated.Add(new NodesCreated((new DateTime(2022, 3, 6)), 100));
            return nodesCreated;
        }

        private List<IDailyRegistration> InitializeDailyRegistrations()
        {
            List<IDailyRegistration> dailyRegistrations = new List<IDailyRegistration>();
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 3)), 100));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 4)), 99));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 5)), 98));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 6)), 97));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 7)), 96));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 8)), 95));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 9)), 94));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 10)), 93));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 11)), 92));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 12)), 91));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 13)), 90));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 14)), 89));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 15)), 88));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 16)), 87));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 17)), 86));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 18)), 85));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 19)), 84));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 20)), 83));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 21)), 82));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 22)), 81));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 23)), 80));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 24)), 79));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 25)), 78));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 26)), 77));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 27)), 76));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 28)), 75));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 29)), 74));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 30)), 73));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 12, 31)), 72));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 1)), 71));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 2)), 70));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 3)), 69));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 4)), 68));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 5)), 67));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 6)), 66));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 7)), 65));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 8)), 64));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 9)), 63));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 10)), 62));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 11)), 61));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 12)), 60));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 13)), 59));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 14)), 58));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 15)), 57));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 16)), 56));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 17)), 55));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 18)), 54));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 19)), 53));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 20)), 52));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 21)), 51));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 22)), 50));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 23)), 49));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 24)), 48));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 25)), 47));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 26)), 46));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 27)), 45));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 28)), 44));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 29)), 43));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 30)), 42));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 1, 31)), 41));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 7)), 40));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 8)), 39));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 9)), 38));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 10)), 37));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 11)), 36));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 12)), 35));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 13)), 34));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 14)), 33));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 15)), 32));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 16)), 31));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 17)), 30));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 18)), 29));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 19)), 28));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 20)), 27));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 21)), 26));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 22)), 25));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 23)), 24));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 24)), 23));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 25)), 22));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 26)), 21));

            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 27)), 20));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 26)), 19));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 27)), 18));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 2, 28)), 17));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 1)), 16));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 2)), 15));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 3)), 14));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 4)), 13));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 5)), 12));
            dailyRegistrations.Add(new DailyRegistration((new DateTime(2022, 3, 6)), 11));
            return dailyRegistrations;
        }

        private List<IConfirmationLink> InitializeConfirmationLinks()
        {
            List<IConfirmationLink> confirmationLinks = new List<IConfirmationLink>();
            return confirmationLinks;
        }

        private List<IView> InitializeViews()
        {
            List<IView> views = new List<IView>();
            views.Add(new View(new DateTime(2022, 3, 6), "UAD", 50, 75.29));
            views.Add(new View(new DateTime(2022, 3, 5), "DeleteAccount", 60, 15.39));
            views.Add(new View(new DateTime(2022, 3, 4), "CreateAccount", 20, 17.43));
            views.Add(new View(new DateTime(2022, 3, 3), "OTPRequest", 40, 1000.45));
            views.Add(new View(new DateTime(2022, 3, 1), "Authenticaton", 30, 69.69));
            views.Add(new View(new DateTime(2022, 2, 28), "Authorization", 70, 46.78));
            return views;
        }
    }
}