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

        public Node(string userHash, long nodeID, long parentNodeID, string nodeTitle, string summary, DateTime timeModified, 
            bool visibility, bool deleted)
        {
            this.UserHash = userHash;
            this.NodeID = nodeID;
            this.ParentNodeID = parentNodeID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.TimeModified = timeModified;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.ExactMatch = false;
            this.Tags = new List<INodeTag>();
            this.TagScore = 0;
            this.RatingScore = 2;
        }

        public Node(Node n)
        {
            this.UserHash = n.UserHash;
            this.NodeID = n.NodeID;
            this.ParentNodeID = n.ParentNodeID;
            this.NodeTitle = n.NodeTitle;
            this.Summary = n.Summary;
            this.TimeModified = n.TimeModified;
            this.Visibility = n.Visibility;
            this.Deleted = n.Deleted;
            this.ExactMatch = false;
            this.Tags = n.Tags;
            this.TagScore = 0;
            //this.RatingScore = n.RatingScore;
            this.RatingScore = 3;
        }

        public Node(string userHash, long nodeID, long nodeParentID, string nodeTitle, string summary, bool visibility, bool deleted)
        {
            this.UserHash = userHash;
            this.NodeID = nodeID;
            this.ParentNodeID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.ExactMatch = false;
            this.TimeModified = DateTime.UtcNow;
            Tags = new List<INodeTag>();
            TagScore = 0;
            RatingScore = 4;
        }
        public Node(long nodeID, long nodeParentID, string nodeTitle, string summary, bool visibility, bool deleted, string userHash)
        {
            this.NodeID = nodeID;
            this.ParentNodeID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.UserHash = userHash;
            this.ExactMatch = false;
            TimeModified = DateTime.UtcNow;
            Tags = new List<INodeTag>();
            TagScore = 0;
            RatingScore = 5;
        }
        public Node(long nodeID, long nodeParentID, string nodeTitle, string summary, DateTime timeModifed, bool visibility, bool deleted, string userhash, double tagScore, double ratingScore)
        {
            this.NodeID = nodeID;
            this.ParentNodeID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.TimeModified = timeModifed;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.UserHash = userhash;
            this.TagScore = 0;
            this.RatingScore = ratingScore;
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