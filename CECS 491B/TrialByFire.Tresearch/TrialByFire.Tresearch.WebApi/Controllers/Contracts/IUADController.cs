using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUADController
    {
        public List<KPI> LoadKPI(DateTime now);
    }
}
