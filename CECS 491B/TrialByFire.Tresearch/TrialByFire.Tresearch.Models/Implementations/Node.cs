using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Node : INode
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }
        public long ParentNodeID { get; set; }
        public string NodeTitle { get; set; }
        public string Summary { get; set; }
        public DateTime TimeModified { get; set; }
        public bool Visibility { get; set; }
        public bool Deleted { get; set; }
        public bool ExactMatch { get; set; }
        public List<INodeTag> Tags { get; set; }
        public double TagScore { get; set; }
        public int Rating { get; set; }

        public Node(string userHash, long nodeID, long parentNodeID, string nodeTitle, string summary, DateTime timeModified,
            bool visibility, bool deleted)
        {
            UserHash = userHash;
            NodeID = nodeID;
            ParentNodeID = parentNodeID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModified;
            Visibility = visibility;
            Deleted = deleted;
            ExactMatch = false;
            Tags = new List<INodeTag>();
            TagScore = 0;
            Rating = 0;
        }
    }
}
