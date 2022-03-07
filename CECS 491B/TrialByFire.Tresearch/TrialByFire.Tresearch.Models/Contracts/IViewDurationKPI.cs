using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IViewDurationKPI : IKPI
	{
		public string result { get; set; }
		public List<IView> views { get; set; }
	}
}

