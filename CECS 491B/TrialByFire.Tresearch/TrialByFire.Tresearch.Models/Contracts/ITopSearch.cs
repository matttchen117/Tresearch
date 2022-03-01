using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ITopSearch
    {
        DateTime topSearchDate { get; set; }

        string searchString { get; set; }

        int searchCount { get; set; }
    }
}