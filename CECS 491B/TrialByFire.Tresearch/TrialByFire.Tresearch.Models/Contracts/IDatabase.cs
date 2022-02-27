using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IDatabase
    {
        IList<IOTPClaim> _claims { get; set; }
        IList<IAccount> _accounts { get; set; }
        IList<INode> _nodes { get; set; }
        IList<ITag> _tags { get; set; }
        IList<INodeTag> _nodeTags { get; set; }
        IList<IRating> _ratings { get; set; }
        IList<ITreeHistory> _treeHistories { get; set; }
        IList<IWebPageKPI> _webPageKPIs { get; set; }
        IList<IDailyRegistrationKPI> _dailyRegistrationKPIs { get; set; }
        IList<IDailyLogin> _dailyLoginKPIs { get; set; }
        IList<ITopSearch> _topSearchesKPIs { get; set; }
        IList<INodesCreated> _nodesCreatedKPIs { get; set; }
        IList<IConfirmationLink> _confirmationLinks { get; set; }

    }
}
