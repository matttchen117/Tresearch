using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Rating : IRating
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }

        public int UserRating { get; set; }

        public Rating(string username, long nodeID, int rating)
        {
            UserHash = username;
            NodeID = nodeID;
            UserRating = rating;
        }
    }
}