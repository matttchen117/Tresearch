using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IUADController
    {
        public Task<ActionResult<IKPI>> LoadKPIAsync(DateTime now);
    }
}
