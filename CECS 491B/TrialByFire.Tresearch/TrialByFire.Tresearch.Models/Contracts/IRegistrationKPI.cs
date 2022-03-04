using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IRegistrationKPI : IKPI
	{
		List<int> registrationCount { get; }
	}
}

