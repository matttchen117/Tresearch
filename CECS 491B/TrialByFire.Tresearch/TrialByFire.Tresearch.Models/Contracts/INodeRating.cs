using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INodeRating
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }

        public int Rating { get; set; }

    }
}