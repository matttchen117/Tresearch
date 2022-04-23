using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INode
    {
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
    }
}