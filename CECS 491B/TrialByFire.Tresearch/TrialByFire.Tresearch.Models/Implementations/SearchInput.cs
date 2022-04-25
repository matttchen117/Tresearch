using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    /// <summary>
    ///     SearchInput: Object that represents input data for Search methods
    /// </summary>
    public class SearchInput : ISearchInput
    {
        public string Search { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public bool RatingHighToLow { get; set; }
        public bool TimeNewToOld { get; set; }
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     public SearchInput():
        ///         Constructor for SearchInput object
        /// </summary>
        /// <param name="search">The phrase that the User has input for the Search</param>
        /// <param name="tags">The tags that the User has selected to fitler by</param>
        /// <param name="filterByRating">Whether or not the User wants the results specifically to be filtered by rating</param>
        /// <param name="filterByTime">Whether or not the User wants to results specifically to be filtered by time</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        public SearchInput(string search, IEnumerable<string> tags, bool filterByRating, bool filterByTime, 
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
