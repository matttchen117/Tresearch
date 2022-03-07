using System;
using TrialByFire.Tresearch.Models.Implementations;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IViewKPI : IKPI
	{
		string result { get; set; }
		List<IView> views { get; set; }
	}
}

