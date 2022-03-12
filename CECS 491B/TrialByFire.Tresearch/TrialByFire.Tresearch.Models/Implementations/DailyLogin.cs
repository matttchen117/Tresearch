using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class DailyLogin : IDailyLogin
    {
        public DateTime loginDate { get; set; }

        public int loginCount { get; set; }

        public DailyLogin()
        {
            loginDate = DateTime.Now;
            loginCount = -1;
        }

        public DailyLogin(DateTime loginDate, int loginCount)
        {
            this.loginDate = loginDate;
            this.loginCount = loginCount;
        }
    }
}