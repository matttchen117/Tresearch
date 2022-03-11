using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INodesCreated
    {
        DateTime nodesCreatedDate { get; set; }

        int nodesCreatedCount { get; set; }
    }
}