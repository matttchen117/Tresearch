using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IPrivateAndPublicController
    {

        public Task<ActionResult<string>> PrivateNodeAsync(List<long> nodes);


        //public Task<ActionResult<string>> PublicNodeAsync(List<long> nodes);
    }
}
