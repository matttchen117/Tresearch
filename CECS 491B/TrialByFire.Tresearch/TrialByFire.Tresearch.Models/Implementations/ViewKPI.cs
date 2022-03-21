using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class ViewKPI : IViewKPI
	{
		public string result { get; set; }
		public List<IView> views { get; set; }
		public ViewKPI()
		{
			result = "";
			views = new List<IView>();
		}
		public ViewKPI(string result, List<IView> views)
		{
			this.result = result;
			this.views = views;
		}
	}
}