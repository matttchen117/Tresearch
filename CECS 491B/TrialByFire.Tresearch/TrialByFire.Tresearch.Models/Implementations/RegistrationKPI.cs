using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class RegistrationKPI : IRegistrationKPI
	{
		List<DailyRegistration> dailyRegistrations { get; }
		public RegistrationKPI(List<DailyRegistration> dailyRegistrations)
		{
			this.dailyRegistrations = dailyRegistrations;
		}
	}
}

