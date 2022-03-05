using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class SearchKPI : ISearchKPI
	{
		List<TopSearch> topSearches { get; }

		public SearchKPI(List<TopSearch> topSearches)
		{
			this.topSearches = topSearches;
		}
	}
}

