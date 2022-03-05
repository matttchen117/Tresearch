using System;
using TrialByFire.Tresearch.Models.Implementations;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IViewKPI : IKPI
	{
		List<View> views { get; set; }
	}
}

