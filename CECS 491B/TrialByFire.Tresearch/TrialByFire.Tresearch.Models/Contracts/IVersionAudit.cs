using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IVersionAudit
    {
        public int VersionNumber { get; set; }

        public DateTime CreationTime { get; set; }

        public long RootNodeID { get; set; }

        public List<INodeHistory> NodeHistories { get; set; }
    }
}
