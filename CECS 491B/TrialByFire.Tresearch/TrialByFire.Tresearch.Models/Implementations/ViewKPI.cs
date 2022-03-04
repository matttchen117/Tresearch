using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewKPI : IViewKPI
	{
		List<string> viewNames { get; }
		List<int> viewCounts { get;  }

		public ViewKPI(List<string> viewNames, List<int> viewCounts)
		{
			this.viewNames = viewNames;
			this.viewCounts = viewCounts;
		}
	}
}

