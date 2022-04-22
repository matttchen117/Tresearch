using System;
using System.Runtime.Serialization;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    [DataContract]
    public class Node : INode
    {
        [DataMember]
        public string UserHash { get; set; }
        [DataMember]
        public long NodeID { get; set; }
        [DataMember]
        public long ParentNodeID { get; set; }
        [DataMember]
        public string NodeTitle { get; set; }
        [DataMember]
        public string Summary { get; set; }
        [DataMember]
        public DateTime TimeModified { get; set; }
        [DataMember]
        public bool Visibility { get; set; }
        [DataMember]
        public bool Deleted { get; set; }
        [DataMember]
        public bool ExactMatch { get; set; }
        [DataMember]
        public List<INodeTag> Tags { get; set; }
        [DataMember]
        public double TagScore { get; set; }
        [DataMember]
        public double RatingScore { get; set; }

        public Node() 
        {
            Tags = new List<INodeTag>();
        }

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
            RatingScore = 0;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is Node)
                {
                    Node node = (Node)obj;
                    return NodeID.Equals(node.NodeID) && Tags.SequenceEqual(node.Tags) && RatingScore.Equals(node.RatingScore);
                }
            }
            return false;
        }
    }
}