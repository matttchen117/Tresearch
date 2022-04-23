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
        public List<TreeNode> Children { get; set; }

        public TreeNode()
        {

            Children = new List<TreeNode>();
        }

        public TreeNode(INode n)
        {
            Children = new List<TreeNode>();
            UserHash = n.UserHash;
            NodeID = n.NodeID;
            ParentNodeID = n.ParentNodeID;
            NodeTitle = n.NodeTitle;
            Summary = n.Summary;
            TimeModified = n.TimeModified;
            Visibility = n.Visibility;
            Deleted = n.Deleted;
            ExactMatch = n.ExactMatch;
            Tags = n.Tags;
            TagScore = n.TagScore;
            RatingScore = n.RatingScore;
        }


    }
}