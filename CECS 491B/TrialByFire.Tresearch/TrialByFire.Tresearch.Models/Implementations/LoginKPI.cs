using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class LoginKPI : ILoginKPI
	{
		public List<DailyLogin> dailyLogins { get; set;  }

		public LoginKPI(List<DailyLogin> dailyLogins)
		{
			this.dailyLogins = dailyLogins;
		}
	}
}

