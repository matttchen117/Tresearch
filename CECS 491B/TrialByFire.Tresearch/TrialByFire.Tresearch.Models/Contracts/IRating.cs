using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IRating
    {
        string Username { get; set; }
        long NodeID { get; set; }

        int UserRating { get; set; }

    }
}