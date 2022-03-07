using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class DailyRegistration : IDailyRegistration
    {
        public DateTime registrationDate { get; set; }

        public int registrationCount { get; set; }

        public DailyRegistration()
        {
            registrationDate = DateTime.Now;
            registrationCount = -1;
        }
        public DailyRegistration(DateTime registrationDate, int registrationCount)
        {
            this.registrationDate = registrationDate;
            this.registrationCount = registrationCount;
        }
    }
}