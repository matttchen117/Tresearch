using System;

namespace TrialByFire.Tresearch.Models
{
    public interface INodesCreated
    {
        DateTime nodeCreationDate { get; set; }

        int nodeCreationCount { get; set; }
    }
}