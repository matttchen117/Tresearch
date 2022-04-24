using System;
using System.Runtime.Serialization;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeTag : INodeTag
    {
        public long NodeID { get; set; }
        public string TagName { get; set; }

        public NodeTag()
        {
        }

        public NodeTag(long nodeID, string tagName)
        {
            NodeID = nodeID;
            TagName = tagName;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is NodeTag)
                {
                    NodeTag nodeTag = (NodeTag)obj;
                    return NodeID.Equals(nodeTag.NodeID) && TagName.Equals(nodeTag.TagName);
                }
            }
            return false;
        }
        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is NodeTag)
                {
                    NodeTag nodeTag = (NodeTag)obj;
                    return TagName.Equals(nodeTag.TagName) && NodeID.Equals(nodeTag.NodeID);
                }
            }
            return false;
        }
    }
}