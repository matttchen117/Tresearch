using System;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ISearchKPI	: IKPI
	{
		List<TopSearch> topSearches { get; set; }
	}
}

