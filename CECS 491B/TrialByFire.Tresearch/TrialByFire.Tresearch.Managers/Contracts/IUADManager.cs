using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
	public interface IUADManager
	{

		List<KPI> LoadKPI(DateTime now);
		Task<List<KPI>> KPIsFetchedAsync(DateTime now);
	}
}

