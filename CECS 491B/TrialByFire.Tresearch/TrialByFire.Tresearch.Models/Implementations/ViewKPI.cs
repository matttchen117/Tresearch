using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewKPI : IViewKPI
	{
		List<View> views { get; }

		public ViewKPI(List<View> views)
		{
			this.views = views; 
		}
	}
}

