using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IDatabase
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
        public IList<INodeRating> NodeRatings { get; set; }
        /*        public IList<ITreeHistory> TreeHistories { get; set; }
                public IList<IWebPageKPI> WebPageKPIs { get; set; }
                public IList<IDailyRegistrationKPI> DailyRegistrationKPIs { get; set; }*/
        public IList<IDailyLogin> DailyLogins { get; set; }
        public IList<ITopSearch> TopSearches { get; set; }
        public IList<INodesCreated> NodesCreated { get; set; }

    }
}
