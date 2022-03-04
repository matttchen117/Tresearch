using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface ISearchKPI : IKPI	
	{
		List<string> searchNames { get; }
		List<int> searchCount { get; }
	}
}

