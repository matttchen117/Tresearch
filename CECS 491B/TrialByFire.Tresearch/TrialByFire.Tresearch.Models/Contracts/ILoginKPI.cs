using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface ILoginKPI : IKPI
	{
		List<int> loginCount { get; }
	}
}

