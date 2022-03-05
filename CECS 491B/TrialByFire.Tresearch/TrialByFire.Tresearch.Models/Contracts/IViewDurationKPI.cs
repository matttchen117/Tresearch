using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IViewDurationKPI : IKPI	
	{
		List<string> viewNames { get; }
		List<int> viewAverage { get; }
	}
}

