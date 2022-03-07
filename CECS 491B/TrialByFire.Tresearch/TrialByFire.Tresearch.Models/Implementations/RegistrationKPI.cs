using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class RegistrationKPI : IRegistrationKPI
	{
		public string result { get; set; }
		public List<IDailyRegistration> dailyRegistrations { get; set; }
		public RegistrationKPI()
		{
			result = "";
			dailyRegistrations = new List<IDailyRegistration>();
		}
		public RegistrationKPI(string result, List<IDailyRegistration> dailyRegistrations)
		{
			this.result = result;
			this.dailyRegistrations = dailyRegistrations;
		}
	}
}

