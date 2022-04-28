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
            RatingScore = 1;
            Tags = new List<INodeTag>();
        }

        public Node(string userhash, long nodeID, long nodeParentID, string nodeTitle, string summary,
            DateTime timeModifed, bool visibility, bool deleted)
        {
            UserHash = userhash;
            NodeID = nodeID;
            ParentNodeID = nodeParentID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModifed;
            Visibility = visibility;
            Deleted = deleted;
            TagScore = 0;
            RatingScore = 0;
            Tags = new List<INodeTag>();
        }

        public Node(long nodeID, long nodeParentID, string nodeTitle, string summary, DateTime timeModifed, bool visibility, bool deleted, string userhash, double tagScore, double ratingScore)
        {
            NodeID = nodeID;
            ParentNodeID = nodeParentID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModifed;
            Visibility = visibility;
            Deleted = deleted;
            UserHash = userhash;
            TagScore = 0;
            RatingScore = ratingScore;
            Tags = new List<INodeTag>();
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