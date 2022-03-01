using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface INode
    {
        long nodeID { get; set; }

        long parentNodeID { get; set; }

        string nodeTitle { get; set; }

        string summary { get; set; }

        string mode { get; set; }

        string accountOwner { get; set; }
    }
}