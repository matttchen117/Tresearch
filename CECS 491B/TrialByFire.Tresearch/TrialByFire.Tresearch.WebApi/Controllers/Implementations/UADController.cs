using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [Route("controller")]
    public class UADController : Controller, IUADController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IUADManager _uadManager { get; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public UADController(ISqlDAO sqlDAO, ILogService logService, IUADManager uadManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _uadManager = uadManager;
        }

        [HttpPost("kpi")]
        public async Task<List<IKPI>> LoadKPIAsync(DateTime now)
        {
            List<IKPI> kpiList = new List<IKPI>();
            kpiList = await _uadManager.LoadKPIAsync(now, _cts.Token);
            return kpiList;
        }
    }
}