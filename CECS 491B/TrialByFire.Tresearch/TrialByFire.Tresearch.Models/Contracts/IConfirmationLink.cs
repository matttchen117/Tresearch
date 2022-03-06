using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IConfirmationLink
    {
        string Username { get; set; }

        Guid UniqueIdentifier { get; set; }

        DateTime Datetime { get; set; }
    }
}