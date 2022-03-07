using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class LoginKPI : ILoginKPI
	{
		public string result { get; set; }
		public List<IDailyLogin> dailyLogins { get; set; }
		public LoginKPI()
		{
			result = "";
			dailyLogins = new List<IDailyLogin>();
		}
		public LoginKPI(string result, List<IDailyLogin> dailyLogins)
		{
			this.result = result;
			this.dailyLogins = dailyLogins;
		}
	}
}

