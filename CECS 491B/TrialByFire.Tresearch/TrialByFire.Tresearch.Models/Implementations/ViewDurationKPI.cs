using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewDurationKPI : IViewDurationKPI
	{
		public string result { get; set; }
		public List<IView> views { get; set; }
		public ViewDurationKPI()
		{
			result = "";
			views = new List<IView>();
		}

		public ViewDurationKPI(string result, List<IView> views)
		{
			this.result = result;
			this.views = views;
		}


	}
}