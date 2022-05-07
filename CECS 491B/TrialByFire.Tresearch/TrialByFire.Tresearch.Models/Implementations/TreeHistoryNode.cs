using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class TreeHistoryNode : ITreeHistoryNode
    {
        public long VersionNumber { get; set; }

        public DateTime VersionCreation { get; set; }

        public string UserHash { get; set; }

        public long NodeID { get; set; }

        public long ParentNodeID { get; set; }

        public string NodeTitle { get; set; }

        public string Summary { get; set; }

        public DateTime TimeModified { get; set; }

        public bool Visibility { get; set; }

        public bool Deleted { get; set; }

        public bool ExactMatch { get; set; }

        public List<INodeTag> Tags { get; set; }

        public double TagScore { get; set; }

        public double RatingScore { get; set; }

        public TreeHistoryNode()
        {
            Tags = new List<INodeTag>();
        }

        public TreeHistoryNode(string userhash, long nodeID, long nodeParentID, string nodeTitle, string summary, 
            DateTime timeModified, List<INodeTag> nodeTags, bool visibility, bool deleted, bool exactMatch, double tagScore, double ratingScore)
        {
            UserHash = userhash;
            NodeID = nodeID;
            NodeTitle = nodeTitle;
            Summary = summary;
            TimeModified = timeModified;
            Visibility = visibility;
            Deleted = deleted;
            ExactMatch = exactMatch;
            Tags = nodeTags;
            TagScore = tagScore;
            RatingScore = ratingScore;
        }
    }
}
