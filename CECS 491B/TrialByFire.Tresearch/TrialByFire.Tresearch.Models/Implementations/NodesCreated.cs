using System;

namespace TrialByFire.Tresearch.Models
{
    public class NodesCreated
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