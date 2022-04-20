using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeRating : INodeRating
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }
        public int Rating { get; set; }

        public NodeRating(string username, long nodeID, int rating)
        {
            UserHash = username;
            NodeID = nodeID;
            Rating = rating;
        }
    }
}