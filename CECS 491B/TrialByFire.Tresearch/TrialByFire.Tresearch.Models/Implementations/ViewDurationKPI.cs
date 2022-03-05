using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewDurationKPI : IViewDurationKPI
	{
		public List<string> viewNames { get; }
		public List<int> viewAverage { get;  }

		public ViewDurationKPI(List<string> viewNames, List<int> viewAverage)
		{
			this.viewNames = viewNames;
			this.viewAverage = viewAverage;
		}
	}
}

