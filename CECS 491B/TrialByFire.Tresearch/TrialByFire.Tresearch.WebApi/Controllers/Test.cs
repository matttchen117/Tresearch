using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test : ControllerBase
    {
        public Test()
        {
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Index()
        {
            if(Thread.CurrentPrincipal != null)
            {
                return new OkObjectResult(Thread.CurrentPrincipal.Identity.Name +
                Thread.CurrentPrincipal.IsInRole("user"));
            }
            else
            {
                return new OkObjectResult("Guest");
            }
        }
    }
}
