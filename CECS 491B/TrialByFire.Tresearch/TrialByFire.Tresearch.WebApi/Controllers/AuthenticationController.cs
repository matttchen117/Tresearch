using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers
{
    public class AuthenticationController : Controller
    {

        public AuthenticationController(ISqlDAO a, ILogService q)
        public IActionResult Index()
        {
            return View();
        }
    }
}
