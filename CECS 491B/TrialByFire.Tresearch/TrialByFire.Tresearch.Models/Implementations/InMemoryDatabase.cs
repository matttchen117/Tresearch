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
        public IList<IConfirmationLink> ConfirmationLinks { get; set; }
        public IList<Tuple<IConfirmationLink, int>> ConfirmationLinksCreated { get; set; }
        public IList<IRecoveryLink> RecoveryLinks { get; set; }
        public IList<Tuple<IConfirmationLink, int>> RecoveryLinksCreated { get; set; }
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
        public IList<IView> Views { get; set; }

        public IList<ILog> AnalyticLogs { get; set; }
        public IList<ILog> ArchiveLogs { get; set; }
        public IList<IUserHashObject> UserHashTable { get; set; }

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
            AnalyticLogs = InitializeAnalyticLogs();
            ArchiveLogs = InitializeArchiveLogs();
            UserHashTable = InitializeUserHashTable();
            RecoveryLinks = InitializeRecoveryLinks();
        }

        /*
            In memory database initialization methods
         */

        private List<IRecoveryLink> InitializeRecoveryLinks()
        {
            List<IRecoveryLink> recoveryLinks = new List<IRecoveryLink>();
            recoveryLinks.Add(new RecoveryLink("pammypoor+recoverService1", "user", DateTime.Now.AddDays(-2), Guid.NewGuid()));

            return recoveryLinks;
        }

        private List<IOTPClaim> InitializeOTPClaims()
        {
            List<IOTPClaim> otpClaims = new List<IOTPClaim>();
            otpClaims.Add(new OTPClaim("drakat7@gmail.com", "ABCdef123", "user", new DateTime(2022, 3, 4, 5, 6, 0)));
            otpClaims.Add(new OTPClaim("drakat7@gmail.com", "ABCdef123", "admin", new DateTime(2022, 3, 4, 5, 6, 0)));
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
            accounts.Add(new UserAccount("drakat7@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("drakat7@gmail.com", "abcDEF123", "admin", true, true));
            // for otp request tests
            accounts.Add(new UserAccount("aarry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("barry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new UserAccount("carry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("darry@gmail.com", "abcDEF123", "user", false, true));
            accounts.Add(new UserAccount("earry@gmail.com", "abcDEF123", "user", false, false));
            accounts.Add(new UserAccount("farry@gmail.com", "abcDEF123", "user", true, true));
            // for authentication tests
            accounts.Add(new UserAccount("garry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("harry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new UserAccount("iarry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new UserAccount("jarry@gmail.com", "abcDEF123", "admin", true, true));
            accounts.Add(new UserAccount("karry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("larry@gmail.com", "abcDEF123", "user", false, true));
            accounts.Add(new UserAccount("marry@gmail.com", "abcDEF123", "user", false, false));
            accounts.Add(new UserAccount("narry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("oarry@gmail.com", "abcDEF123", "user", true, true));
            accounts.Add(new UserAccount("parry@gmail.com", "abcDEF123", "user", true, true));

            /*            Viet adding new accounts
                        [InlineData("grizzly@gmail.com", "user", "success")]
                        [InlineData("salewa@gmail.com", "admin", "success")]
                        Accounts for AccountDeletionController*/
            accounts.Add(new UserAccount("grizzly@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new UserAccount("salewa@gmail.com", "asdfasdf123", "admin", true, true));

            /*            Accounts for AccountDeletionManager
                        [InlineData("trizip@gmail.com", "user", "success")]
                        [InlineData("switchblade@gmail.com", "admin", "success")]*/
            accounts.Add(new UserAccount("trizip@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new UserAccount("switchblade@gmail.com", "asdfasdf123", "admin", true, true));
            /*            Accounts for AccountDeletionService
                        [InlineData("altyn@gmail.com", "user", "success")]
                        [InlineData("ryst@gmail.com", "admin", "success")]*/

            accounts.Add(new UserAccount("altyn@gmail.com", "asdfasdf", "user", true, true));
            accounts.Add(new UserAccount("ryst@gmail.com", "asdfasdf123", "admin", true, true));
            return accounts;
        }

        private List<INode> InitializeNodes()
        {
            List<INode> nodes = new List<INode>();
            nodes.Add(new Node(100000, 100001, "Title 1", "Summary 1", false, "larry@gmail.com"));
            nodes.Add(new Node(100001, 100002, "Title 2", "Summary 2", false, "larry@gmail.com"));
            nodes.Add(new Node(100002, 100003, "Title 3", "Summary 3", false, "larry@gmail.com"));
            nodes.Add(new Node(100003, 100004, "Title 4", "Summary 4", true, "larry@gmail.com"));
            nodes.Add(new Node(100004, 100004, "Title 5", "Summary 5", true, "larry@gmail.com"));

            nodes.Add(new Node(200000, 200001, "Title 1", "Summary 1", false, "billy@yahoo.com"));
            nodes.Add(new Node(200001, 200002, "Title 2", "Summary 2", false, "billy@yahoo.com"));
            nodes.Add(new Node(200002, 200003, "Title 3", "Summary 3", false, "billy@yahoo.com"));
            nodes.Add(new Node(200003, 200004, "Title 4", "Summary 4", true, "billy@yahoo.com"));
            nodes.Add(new Node(200004, 200004, "Title 5", "Summary 5", true, "billy@yahoo.com"));

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
            /*dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 1), 1));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 2), 2));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 3), 3));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 4), 4));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 5), 5));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 6), 6));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 7), 7));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 8), 8));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 9), 9));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 10), 10));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 11), 11));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 12), 12));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 13), 13));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 14), 14));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 15), 15));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 16), 16));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 17), 17));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 18), 18));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 19), 19));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 20), 20));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 21), 21));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 22), 22));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 23), 23));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 24), 24));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 25), 25));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 26), 26));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 27), 27));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 28), 28));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 29), 29));
            dailyLogins.Add(new DailyLogin(new DateTime(2000, 1, 30), 30));*/
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
            /*topSearches.Add(new TopSearch(new DateTime(2000, 1, 1), "test1", 1));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 2), "test2", 2));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 3), "test3", 3));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 4), "test4", 4));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 5), "test5", 5));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 6), "test6", 6));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 7), "test7", 7));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 8), "test8", 8));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 9), "test9", 9));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 10), "test10", 10));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 11), "test11", 11));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 12), "test12", 12));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 13), "test13", 13));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 14), "test14", 14));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 15), "test15", 15));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 16), "test16", 16));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 17), "test17", 17));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 18), "test18", 18));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 19), "test19", 19));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 20), "test20", 20));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 21), "test21", 21));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 22), "test22", 22));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 23), "test23", 23));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 24), "test24", 24));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 25), "test25", 25));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 26), "test26", 26));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 27), "test27", 27));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 28), "test28", 28));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 29), "test29", 29));
            topSearches.Add(new TopSearch(new DateTime(2000, 1, 30), "test30", 30));*/
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
            /*nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 1), 1));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 2), 2));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 3), 3));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 4), 4));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 5), 5));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 6), 6));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 7), 7));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 8), 8));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 9), 9));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 10), 10));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 11), 11));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 12), 12));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 13), 13));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 14), 14));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 15), 15));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 16), 16));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 17), 17));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 18), 18));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 19), 19));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 20), 20));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 21), 21));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 22), 22));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 23), 23));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 24), 24));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 25), 25));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 26), 26));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 27), 27));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 28), 28));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 29), 29));
            nodesCreated.Add(new NodesCreated(new DateTime(2000, 1, 30), 30));*/
            return nodesCreated;
        }

        private IList<IDailyRegistration> InitializeDailyRegistrations()
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

        private List<ILog> InitializeAnalyticLogs()
        {
            List<ILog> logs = new List<ILog>();
            return logs;
        }

        private List<ILog> InitializeArchiveLogs()
        {
            List<ILog> logs = new List<ILog>();
            return logs;
        }

        private List<IUserHashObject> InitializeUserHashTable()
        {
            List<IUserHashObject> userHashObjects = new List<IUserHashObject>();
            userHashObjects.Add(new UserHashObject("drakat7@gmail.com", "user", "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6"));
            userHashObjects.Add(new UserHashObject("drakat7@gmail.com", "admin", "D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32"));
            userHashObjects.Add(new UserHashObject("aarry@gmail.com", "user", "AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D"));
            userHashObjects.Add(new UserHashObject("barry@gmail.com", "admin", "E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159"));
            userHashObjects.Add(new UserHashObject("carry@gmail.com", "user", "CB3C47AD9DCEFA2CAC50D472CAFA954F00476EA58B11E8F2CD32E46F4C3DC1C990867A78D484BB25E8FF2FB44CE9F99F356E275E7E2684FFF714BAC11B663FBF"));
            userHashObjects.Add(new UserHashObject("darry@gmail.com", "user", "F188DBA08CB0C30B6FA09BD87146F15CA3D08AAABB501C5D439F8D8A33E76EA6E701E066C8DB8870895A29BCF72004E35BC0B827912F12E119BD218F5BF0BCCE"));
            userHashObjects.Add(new UserHashObject("earry@gmail.com", "user", "E26732DF9CF51AC7668A5BA02B116070F16F11D0BE2A3BFE73F2B87D429DE5C96754D976FE474EE67A59887E8C5646AE177144E37A0D4B339CD9B3D16107F3D0"));
            userHashObjects.Add(new UserHashObject("farry@gmail.com", "user", "8F88CE35DBF55AEFCBE6CF68CEB21CDD04B2D8B9FFC0A5024F88AF94EC5E2FFAB231E549FF0E5A50F08302CFE2B2925A6D368B49D22794B710AC6F78685E0A0C"));
            userHashObjects.Add(new UserHashObject("garry@gmail.com", "user", "8C89E7886643911D171624EEFDF875F6B45C0052A761A6A713E9D26EFCF66F9D47ADD4899E98C6A8525CCB7D68F9BAB1EF1A75D4F1558726103FF0BE7B6A32B2"));
            userHashObjects.Add(new UserHashObject("harry@gmail.com", "admin", "D8D551BF832A6BEF37557C4DEC5321EE15AD69562991FC0EEBD54711CDD58931F31661635CD64518B327ADAF8CD788A5293A0E0BD528C6D0A1EEA7F282AA9143"));
            userHashObjects.Add(new UserHashObject("iarry@gmail.com", "admin", "C0EB8D659C0129F28EA863560115E7FB30AF64739F680A86C28850EC5BFCCD29BD9F0520899857B9BD72063F575737DFC61DCBD75DDE9F8A93E8E15ABD923F0F"));
            userHashObjects.Add(new UserHashObject("jarry@gmail.com", "admin", "3CC3F278EEDCA0885C1043D16B11527C746F881A263044440D5F8FA7C93FB8A07E0BDE21A1F3395E60BFC231B96EA08809B5A2043710AC9153E48699CA292F86"));
            userHashObjects.Add(new UserHashObject("karry@gmail.com", "user", "66B41BF3C0D1C7E958C76409230A95EC1A0F33CBC376AC146CE0A85967590B5C8DE9C2EEFDB488072EB1DE456D965078ED283EE42F0076C91BD06BB66640951B"));
            userHashObjects.Add(new UserHashObject("larry@gmail.com", "user", "7F4BA1D5E240AE1B8F8B5E5F3ECF1833ABE41B5A77B83383EF26868D77F0567EF6CEA2A3E38D54CC8AFB9E1E79DA7C215A4CE530932C13278FBBAEA7D98EDF18"));
            userHashObjects.Add(new UserHashObject("marry@gmail.com", "user", "13CE5ABC3473B264F1A413C78855DBFFBCC37C52153EF65F6FA790D66CC6146EBC2CFBEF0554C31BAB65F35A05F19EB6FB833F0393A7E07CC67B85E8B2272434"));
            userHashObjects.Add(new UserHashObject("narry@gmail.com", "user", "FCB748D5CC578967ECDC2E7DA7A5128AD7AE0198C460EDE2205161CAC3F794484E5BE6AB93B0ED10E6E766461AC37E754F549B6D0B523C2903FC1CD14DEAA3D9"));
            userHashObjects.Add(new UserHashObject("oarry@gmail.com", "user", "700F13E5C73A02ED040D5F5D06BCD381C05639967F3B2F81F8BB6ED72CF89C4FB6B8BB67C9C3D4318A5260F3BE8CAAF9AD8A65210CA7CC751F7B66AE7DEE7A48"));
            userHashObjects.Add(new UserHashObject("parry@gmail.com", "user", "531267DC5B626D7E54C51CB1F9B5A4E786FBDB53C0F62A851DDE68C7F9A402AA852B3654BFDE4C94B68603C603F88AC0F0FF67A4A4E7849EB244F1DF5237CEF1"));
            userHashObjects.Add(new UserHashObject("qarry@gmail.com", "user", "96077B964424761F51F2A59340F0E40ADB225C69087AD599D13E8622BEA4A0EABBD0BFEEBAAF9CD4F9CDFA2AE765630B9AD9C0668E5BF9C5F1CAD6EA7F96FE92"));
            userHashObjects.Add(new UserHashObject("rarry@gmail.com", "user", "104FE34A615CA8D52BDF5AF486999767CD43DEF539FD52973B5CFB6A08753AB0382AD2111530F50B5614B9E6F6333CCB3DA65F9E25E81F5B8305CFAAC2A76303"));
            userHashObjects.Add(new UserHashObject("sarry@gmail.com", "admin", "4901F9B280F604E861C14451768BEF3F10E8DD9808662B76BB8BFC9A12D0DF2D2B8FC5162D0271FED66057B9F0000B1D49AC2110D168CE37CEF47E1A20304B85"));

            return userHashObjects;
        }
    }
}