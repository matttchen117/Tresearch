using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IRegistrationKPI : IKPI
	{
		string result { get; set; }
		List<IDailyRegistration> dailyRegistrations { get; set; }
	}
}

