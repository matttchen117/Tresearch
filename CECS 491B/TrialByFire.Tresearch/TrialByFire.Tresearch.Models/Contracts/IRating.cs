using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IRating
    {
        string username { get; set; }
        long nodeID { get; set; }

        int rating { get; set; }

    }
}