using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("controller")]
    public class UADController : Controller, IUADController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IUADManager _uadManager { get; }
        private CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        public UADController(ISqlDAO sqlDAO, ILogService logService, IUADManager uadManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _uadManager = uadManager;
        }

        [HttpPost("kpi")]
        public async Task<List<IKPI>> LoadKPIAsync(DateTime now)
        {
            List<IKPI> result = new List<IKPI>();
            try
            {
                result = await _uadManager.LoadKPIAsync(now, _cts.Token).ConfigureAwait(false);
                return result;
            }
            catch(OperationCanceledException tce)
            {
                result.Add(new KPI(tce.Message));
                return result;
            }
            catch(Exception ex)
            {
                result.Add(new KPI(ex.Message));
                return result;
            }
        }
    }
}