using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class VersionAudit : IVersionAudit
    {
        public int VersionNumber { get; set; }

        public DateTime CreationTime { get; set; }

        public long RootNodeID { get; set; }

        public List<INodeHistory> NodeHistories { get; set; }

        public VersionAudit(int versionNumber, DateTime creationTime, long rootNodeID, List<INodeHistory> nodeHistories) { 
            VersionNumber = versionNumber;
            CreationTime = creationTime;
            RootNodeID = rootNodeID;
            NodeHistories = nodeHistories;
        }
    }
}
