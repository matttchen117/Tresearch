using System;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
	public interface IUADService
	{
		List<KPI> LoadKPI(DateTime now);
	}
}

