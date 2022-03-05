using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class SearchKPI : ISearchKPI
	{
		public List<TopSearch> topSearches { get; set; }

		public SearchKPI(List<TopSearch> topSearches)
		{
			this.topSearches = topSearches;
		}
	}
}

