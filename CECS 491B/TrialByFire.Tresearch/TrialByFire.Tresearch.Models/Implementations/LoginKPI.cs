using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class LoginKPI : ILoginKPI
	{
		List<DailyLogin> dailyLogins { get; }

		public LoginKPI(List<DailyLogin> dailyLogins)
		{
			this.dailyLogins = dailyLogins;
		}
	}
}

