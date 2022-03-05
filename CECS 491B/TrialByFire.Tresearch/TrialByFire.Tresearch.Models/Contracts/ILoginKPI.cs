using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface ILoginKPI : IKPI
	{
		List<DailyLogin> dailyLogins { get; }
	}
}

