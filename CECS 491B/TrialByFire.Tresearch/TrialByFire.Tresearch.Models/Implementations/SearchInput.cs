using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class SearchInput : ISearchInput
    {
        public string Search { get; set; }
        public List<string> Tags { get; set; }
        public bool RatingHighToLow { get; set; }
        public bool TimeNewToOld { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public SearchInput(string search, List<string> tags, bool filterByRating, bool filterByTime, 
            CancellationToken cancellationToken = default)
        {
            Search = search;
            Tags = tags;
            RatingHighToLow = filterByRating;
            TimeNewToOld = filterByTime;
            CancellationToken = cancellationToken;
        }
    }
}
