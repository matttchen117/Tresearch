using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Node : INode
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }
        public long NodeParentID { get; set; }
        public string NodeTitle { get; set; }
        public string Summary { get; set; }
        public DateTime TimeModified { get; set; }
        public bool Visibility { get; set; }
        public bool Deleted { get; set; }
        public bool ExactMatch { get; set; }
        public List<INodeTag> Tags { get; set; }
        public double TagScore { get; set; }
        public double RatingScore { get; set; }

        public Node()
        {
            //Tags = new List<INodeTag>();
        }

        public Node(string userHash, long nodeID, long nodeParentID, string nodeTitle, string summary, DateTime timeModified,
            bool visibility, bool deleted)
        {
            UserHash = userHash;
            NodeID = nodeID;
            NodeParentID = nodeParentID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModified;
            Visibility = visibility;
            Deleted = deleted;
            ExactMatch = false;
            Tags = new List<INodeTag>();
            TagScore = 1;
            RatingScore = 0;
        }

        public Node(Node n)
        {
            this.UserHash = n.UserHash;
            this.NodeID = n.NodeID;
            this.NodeParentID = n.NodeParentID;
            this.NodeTitle = n.NodeTitle;
            this.Summary = n.Summary;
            this.TimeModified = n.TimeModified;
            this.Visibility = n.Visibility;
            this.Deleted = n.Deleted;
            this.ExactMatch = false;
            this.Tags = n.Tags;
            this.TagScore = 2;
            this.RatingScore = n.RatingScore;
        }

        public Node(string userHash, long nodeID, long nodeParentID, string nodeTitle, string summary, bool visibility, bool deleted)
        {
            this.UserHash = userHash;
            this.NodeID = nodeID;
            this.NodeParentID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.ExactMatch = false;
            this.TimeModified = DateTime.UtcNow;
            Tags = new List<INodeTag>();
            TagScore = 3;
            RatingScore = 0;
        }
        public Node(long nodeID, long nodeParentID, string nodeTitle, string summary, bool visibility, bool deleted, string userHash)
        {
            this.NodeID = nodeID;
            this.NodeParentID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.UserHash = userHash;
            this.ExactMatch = false;
            TimeModified = DateTime.UtcNow;
            Tags = new List<INodeTag>();
            TagScore = 4;
            RatingScore = 0;
        }
        public Node(long nodeID, long nodeParentID, string nodeTitle, string summary, DateTime timeModifed, bool visibility, bool deleted, string userhash, double tagScore, double ratingScore)
        {
            this.NodeID = nodeID;
            this.NodeParentID = nodeParentID;
            this.NodeTitle = nodeTitle;
            this.Summary = summary;
            this.TimeModified = timeModifed;
            this.Visibility = visibility;
            this.Deleted = deleted;
            this.UserHash = userhash;
            this.TagScore = 5;
            this.RatingScore = ratingScore;
            Tags = new List<INodeTag>();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if(obj is NodeTag)
                {
                    Node node = (Node)obj;
                    return NodeID.Equals(node.NodeID) && Tags.SequenceEqual(node.Tags) && RatingScore.Equals(node.RatingScore);
                }
            }
            return false;     
        }
    }
}