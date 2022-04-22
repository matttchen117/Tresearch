using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ISearchInput
    {
        public string Search { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public bool RatingHighToLow { get; set; }
        public bool TimeNewToOld { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
