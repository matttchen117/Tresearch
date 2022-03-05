using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface ILoginKPI : IKPI
	{
		public List<DailyLogin> dailyLogins { get; set; }
	}
}

