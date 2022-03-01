using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IConfirmationLink
    {
        string username { get; set; }

        Guid uniqueIdentifier { get; set; }

        DateTime timestamp { get; set; }
    }
}