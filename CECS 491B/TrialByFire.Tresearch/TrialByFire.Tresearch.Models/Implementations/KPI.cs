using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class KPI : IKPI
	{
		public string result { get; set; } 
		public KPI(string result)
		{
			this.result = result;
		}
	}
}

