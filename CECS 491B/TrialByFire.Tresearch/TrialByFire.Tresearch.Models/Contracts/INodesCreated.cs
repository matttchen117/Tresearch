using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INodesCreated
    {
        DateTime nodeCreationDate { get; set; }

        int nodeCreationCount { get; set; }
    }
}