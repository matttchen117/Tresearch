using System;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class DailyRegistration
    {
        public DateTime registrationDate { get; set; }

        public int registrationCount { get; set; }

        public DailyRegistration(DateTime registrationDate, int registrationCount)
        {
            this.registrationDate = registrationDate;
            this.registrationCount = registrationCount;
        }
    }
}