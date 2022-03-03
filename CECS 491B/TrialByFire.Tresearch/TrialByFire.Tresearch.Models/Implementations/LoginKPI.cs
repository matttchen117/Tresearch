using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class LoginKPI : ILoginKPI
	{
		List<int> loginCount { get; }

		public LoginKPI(List<int> loginCount)
		{
			this.loginCount = loginCount;
		}
	}
}

