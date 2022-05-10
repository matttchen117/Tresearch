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
        public DateTime CreationDate { get; set; }

        public long RootNodeID { get; set; }

        public List<INodeHistory> NodeHistories { get; set; }

        public VersionAudit(DateTime creationDate, long rootNodeID, List<INodeHistory> nodeHistories) 
        {
            CreationDate = creationDate;
            RootNodeID = rootNodeID;
            NodeHistories = nodeHistories;
        }
    }
}
