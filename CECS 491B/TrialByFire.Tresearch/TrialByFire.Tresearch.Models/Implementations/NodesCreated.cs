using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodesCreated : INodesCreated
    {
        public DateTime nodeCreationDate { get; set; }

        public int nodeCreationCount { get; set; }

        public NodesCreated(DateTime nodeCreationDate, int nodeCreationCount)
        {
            this.nodeCreationDate = nodeCreationDate;
            this.nodeCreationCount = nodeCreationCount;
        }
    }
}