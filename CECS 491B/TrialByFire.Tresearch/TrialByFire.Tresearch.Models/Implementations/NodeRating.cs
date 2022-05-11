using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeRating : INodeRating, IEquatable<NodeRating>
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

        public bool Equals(NodeRating rating)
        {
            return UserHash.Equals(rating.UserHash) && NodeID.Equals(rating.NodeID) && Rating.Equals(rating.Rating);
        }
    }
}