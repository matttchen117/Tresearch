using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeHistory : INodeHistory
    {
        public long NodeID { get; set; }

        public long ParentNodeID { get; set; }

        public string NodeTitle { get; set; }

        public string Summary { get; set; }

        public DateTime TimeModified { get; set; }

        public bool Visibility { get; set; }

        public bool Deleted { get; set; }

        public NodeHistory(long nodeID, long parentNodeID, string nodeTitle, string summary, DateTime timeModified, bool visibility, bool deleted)
        {
            NodeID = nodeID;
            ParentNodeID = parentNodeID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModified;
            Visibility = visibility;
            Deleted = deleted;
        }
    }
}
