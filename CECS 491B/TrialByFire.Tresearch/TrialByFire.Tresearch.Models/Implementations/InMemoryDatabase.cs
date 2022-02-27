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
        public IList<IOTPClaim> _claims { get; set; }
        public IList<IAccount> _accounts { get; set; }
        public IList<INode> _nodes { get; set; }
        public IList<ITag> _tags { get; set; }
        public IList<INodeTag> _nodeTags { get; set; }
        public IList<IRating> _ratings { get; set; }
        public IList<ITreeHistory> _treeHistories { get; set; }
        public IList<IWebPageKPI> _webPageKPIs { get; set; }
        public IList<IDailyRegistrationKPI> _dailyRegistrationKPIs { get; set; }
        public IList<IDailyLogin> _dailyLoginKPIs { get; set; }
        public IList<ITopSearch> _topSearchesKPIs { get; set; }
        public IList<INodesCreated> _nodesCreatedKPIs { get; set; }
        public IList<IConfirmationLink> _confirmationLinks { get; set; }

        public InMemoryDatabase()
        {
            _claims = new List<IOTPClaim>();
            _accounts = new List<IAccount>();
        }
    }
}
