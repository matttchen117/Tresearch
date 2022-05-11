using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class KPI : IKPI
	{
		public string result { get; set; }
		public List<int> dailyLogins { get; set; }
		public List<int> dailyRegistrations { get; set; }
		public List<int> dailyNodesCreated { get; set; }
		public KPI()
        {
			result = "";
			dailyLogins = new List<int>();
			dailyRegistrations = new List<int>();
			dailyNodesCreated = new List<int>();
        }

		public KPI(string result, List<int> dLogins, List<int> dReg, List<int> dNC) 
		{
			this.result = result;
			this.dailyLogins = dLogins;
			this.dailyRegistrations = dReg;
			this.dailyNodesCreated = dNC;
		}

	}
}