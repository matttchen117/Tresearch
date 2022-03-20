using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
	public interface IUADManager
	{
		Task<List<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellation = default);
	}
}

