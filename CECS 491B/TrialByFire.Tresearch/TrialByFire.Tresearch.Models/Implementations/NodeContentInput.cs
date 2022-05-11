using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeContentInput : INodeContentInput
    {
        public string Owner { get; set; }
        public long NodeID { get; set; }
        public string NodeTitle { get; set; }
        public string Summary { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public NodeContentInput(string owner, long nodeID, string nodeTitle, string summary, 
            CancellationToken cancellationToken = default)
        {
            Owner = owner;
            NodeID = nodeID;
            NodeTitle = nodeTitle;
            Summary = summary;
            CancellationToken = cancellationToken;
        }
    }
}
