using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodesCreated : INodesCreated
    {
        public DateTime nodesCreatedDate { get; set; }

        public int nodesCreatedCount { get; set; }

        public NodesCreated()
        {
            nodesCreatedDate = DateTime.Now.ToUniversalTime();
            nodesCreatedCount = -1;
        }

        public NodesCreated(DateTime nodesCreatedDate, int nodesCreatedCount)
        {
            this.nodesCreatedDate = nodesCreatedDate;
            this.nodesCreatedCount = nodesCreatedCount;
        }
    }
}