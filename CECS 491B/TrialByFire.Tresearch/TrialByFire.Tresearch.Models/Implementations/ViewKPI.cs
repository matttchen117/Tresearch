using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewKPI : IViewKPI
	{
		public List<View> views { get; set; }

		public ViewKPI(List<View> views)
		{
			this.views = views; 
		}
	}
}

