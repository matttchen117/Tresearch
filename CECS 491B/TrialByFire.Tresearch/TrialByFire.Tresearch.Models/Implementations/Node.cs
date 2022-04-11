using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Node : INode
    {
        public long nodeID { get; set; }

        public long parentNodeID { get; set; }

        public string nodeTitle { get; set; }

        public string summary { get; set; }

        public bool visibility { get; set; }

        public string accountOwner { get; set; }

        public Node(long nodeID, long parentNodeID, string nodeTitle, string summary, bool visibility, string accountOwner)
        {
            this.nodeID = nodeID;
            this.parentNodeID = parentNodeID;
            this.nodeTitle = nodeTitle;
            this.summary = summary;
            this.visibility = visibility;
            this.accountOwner = accountOwner;
        }

        public Node(Node n)
        {
            this.nodeID = n.nodeID;
            this.parentNodeID = n.parentNodeID;
            this.nodeTitle = n.nodeTitle;
            this.summary = n.summary;
            this.visibility = n.visibility;
            this.accountOwner = n.accountOwner;
        }
    }
}