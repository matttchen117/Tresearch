using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUADController
    {
        public Task<List<IKPI>> LoadKPIAsync(DateTime now);
    }
}
