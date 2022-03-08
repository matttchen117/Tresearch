using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUADController
    {
        public List<IKPI> LoadKPI();
    }
}
