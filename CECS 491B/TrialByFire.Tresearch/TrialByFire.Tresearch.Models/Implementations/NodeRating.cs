using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeRating : INodeRating
    {
        public string UserHash { get; set; }
        public long NodeID { get; set; }
        public int Rating { get; set; }

        public NodeRating()
        {
        }

        public NodeRating(string username, long nodeID, int rating)
        {
            UserHash = username;
            NodeID = nodeID;
            Rating = rating;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj == null))
            {
                if (obj is NodeRating)
                {
                    NodeRating nodeRating = (NodeRating)obj;
                    return UserHash.Equals(nodeRating.UserHash) && NodeID.Equals(nodeRating.NodeID)
                        && Rating.Equals(nodeRating.Rating);
                }
            }
            return false;
        }
    }
}