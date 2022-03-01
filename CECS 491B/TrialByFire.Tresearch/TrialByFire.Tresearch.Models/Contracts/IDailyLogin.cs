using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IDailyLogin
    {
        DateTime loginDate { get; set; }

        int loginCount { get; set; }
    }
}