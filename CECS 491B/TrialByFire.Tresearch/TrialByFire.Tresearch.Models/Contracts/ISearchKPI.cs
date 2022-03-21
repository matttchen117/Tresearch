using System;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface ISearchKPI : IKPI
	{
		string result { get; set; }
		List<ITopSearch> topSearches { get; set; }
	}
}

