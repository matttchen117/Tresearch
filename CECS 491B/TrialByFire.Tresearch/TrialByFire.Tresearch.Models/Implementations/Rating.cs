using System;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Rating
    {
        public string username { get; set; }
        public long nodeID { get; set; }

        public int rating { get; set; }

        public Rating(string username, long nodeID, int rating)
        {
            this.username = username;
            this.nodeID = nodeID;
            this.rating = rating;
        }
    }
}