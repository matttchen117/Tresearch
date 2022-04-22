using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class TreeNode : Node
    {
        public List<INode> Children { get; set; }

        public TreeNode()
        {
            Children = new List<INode>();
        }

        public TreeNode(INode n)
        {
            Children = new List<INode>();
            this.UserHash = n.UserHash;
            this.NodeID = n.NodeID;
            this.NodeParentID = n.NodeParentID;
            this.NodeTitle = n.NodeTitle;
            this.Summary = n.Summary;
            this.TimeModified = n.TimeModified;
            this.Visibility = n.Visibility;
            this.Deleted = n.Deleted;
            this.ExactMatch = n.ExactMatch;
            this.Tags = n.Tags;
            this.TagScore = n.TagScore;
            this.RatingScore = n.RatingScore;
        }


    }
}