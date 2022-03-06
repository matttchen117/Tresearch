using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class TopSearch : ITopSearch
    {
        public DateTime topSearchDate { get; set; }

        public string searchString { get; set; }

        public int searchCount { get; set; }

        public TopSearch(DateTime topSearchDate, string searchString, int searchCount)
        {
            this.topSearchDate = topSearchDate;
            this.searchString = searchString;
            this.searchCount = searchCount;
        }
    }
}