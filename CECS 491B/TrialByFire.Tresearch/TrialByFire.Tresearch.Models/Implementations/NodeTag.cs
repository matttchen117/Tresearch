using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeTag : INodeTag
    {
        public long NodeID { get; set; }
        public string TagName { get; set; }

        public NodeTag(long nodeID, string tagName)
        {
            this.NodeID = nodeID;
            this.TagName = tagName;
        }
    }
}