using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test : ControllerBase
    {
        private IRolePrincipal _rolePrincipal { get; }
        public Test(IRolePrincipal rolePrincipal)
        {
            _rolePrincipal = rolePrincipal;
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Index()
        {
            return new OkObjectResult(_rolePrincipal.RoleIdentity.Username + 
                _rolePrincipal.RoleIdentity.AuthorizationLevel);
        }
    }
}
