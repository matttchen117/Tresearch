using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ITreeHistoryNode
    {
        public long VersionNumber { get; set; }

        public DateTime VersionCreation { get; set; }

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
