using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class SearchKPI : ISearchKPI
	{
		public string result { get; set; }
		public List<ITopSearch> topSearches { get; set; }

		public SearchKPI()
		{
			result = "";
			this.topSearches = new List<ITopSearch>();

		}
		public SearchKPI(string result, List<ITopSearch> topSearches)
		{
			this.result = result;
			this.topSearches = topSearches;
		}
	}
}

