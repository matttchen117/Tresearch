using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface IRegistrationKPI : IKPI
	{
		List<DailyRegistration> dailyRegistrations { get; set; }
	}
}

