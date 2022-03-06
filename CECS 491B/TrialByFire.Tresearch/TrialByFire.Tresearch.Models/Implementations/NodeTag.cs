using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeTag : INodeTag
    {
        public long nodeID { get; set; }
        public string tagName { get; set; }

        public NodeTag(long nodeID, string tagName)
        {
            this.nodeID = nodeID;
            this.tagName = tagName;
        }
    }
}