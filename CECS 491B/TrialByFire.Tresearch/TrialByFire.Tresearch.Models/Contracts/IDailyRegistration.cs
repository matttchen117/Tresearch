using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IDailyRegistration
    {
        DateTime registrationDate { get; set; }

        int registrationCount { get; set; }
    }
}