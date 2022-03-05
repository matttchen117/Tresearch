using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class SearchKPI : ISearchKPI
	{
		public List<string> searchNames { get; }
		public List<int> searchCount { get;  }

		public SearchKPI(List<string> searchNames, List<int> searchCount)
		{
			this.searchNames = searchNames;
			this.searchCount = searchCount;
		}
	}
}

