using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RegistrationKPI : IRegistrationKPI
	{
		public List<DailyRegistration> dailyRegistrations { get; set; }
		public RegistrationKPI(List<DailyRegistration> dailyRegistrations)
		{
			this.dailyRegistrations = dailyRegistrations;
		}
	}
}

