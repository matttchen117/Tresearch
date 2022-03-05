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
        public IList<INode> Nodes { get; set; }
        public IList<ITag> Tags { get; set; }
        public IList<INodeTag> NodeTags { get; set; }
        public IList<IRating> Ratings { get; set; }
        // IList<ITreeHistory> TreeHistories { get; set; }
        //public IList<IWebPageKPI> WebPageKPIs { get; set; }
        //public IList<IDailyRegistrationKPI> DailyRegistrationKPIs { get; set; }
        public IList<IDailyLogin> DailyLoginKPIs { get; set; }
        public IList<ITopSearch> TopSearchesKPIs { get; set; }
        public IList<INodesCreated> NodesCreatedKPIs { get; set; }
        public IList<IConfirmationLink> ConfirmationLinks { get; set; }
        public IList<ILoginKPI> LoginKPIs { get; set; }
        public IList<INodeKPI> NodeKPIs { get; set; }
        public IList<IRegistrationKPI> RegistrationKPIs { get; set; }
        public IList<ISearchKPI> SearchKPIs { get; set; }
        public IList<IViewKPI> ViewKPIs { get; set; }
        public IList<IViewDurationKPI> ViewDurationKPIs { get; set; }
        public IList<IRolePrincipal> RolePrincipals { get; set; }

    }
}
