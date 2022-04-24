using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Rating : IRating
    {
        public string Username { get; set; }
        public long NodeID { get; set; }

        public int UserRating { get; set; }

        public Rating(string username, long nodeID, int rating)
        {
            Username = username;
            NodeID = nodeID;
            UserRating = rating;
        }
    }
}