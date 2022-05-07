using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INodeContentInput
    {
        public string Owner { get; set; }
        public long NodeID { get; set; }
        public string NodeTitle { get; set; }
        public string Summary { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
