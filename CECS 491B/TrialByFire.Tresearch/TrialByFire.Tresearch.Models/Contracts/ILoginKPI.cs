using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ILoginKPI : IKPI
	{
		string result { get; set; }
		public List<IDailyLogin> dailyLogins { get; set; }
	}
}

