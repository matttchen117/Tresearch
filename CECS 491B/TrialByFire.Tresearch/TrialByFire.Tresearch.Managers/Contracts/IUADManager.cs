using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
	public interface IUADManager
	{
		Task<IResponse<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellationToken = default);
	}
}

