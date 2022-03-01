using System;

namespace TrialByFire.Tresearch.Models
{
    public class DailyLogin
    {
        public DateTime loginDate { get; set; }
        
        public int loginCount { get; set; }

        public DailyLogin(DateTime loginDate, int loginCount)
        {
            this.loginDate = loginDate;
            this.loginCount = loginCount;
        }
    }
}