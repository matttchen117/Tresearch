using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IViewKPI : IKPI
	{
		List<string> viewNames { get; }
		List<int> viewCount { get;  }
	}
}

