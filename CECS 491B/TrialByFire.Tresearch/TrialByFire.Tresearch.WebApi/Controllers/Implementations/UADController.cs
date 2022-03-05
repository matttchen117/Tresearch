using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrailByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class UADController : Controller, IUADController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IUADManager _uadManager { get; }
        public UADController(ISqlDAO sqlDAO, ILogService logService, IUADManager uadManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _uadManager = uadManager;
        }

        public List<IKPI> LoadKPI(DateTime now)
        {
            /*List<KPI> results = _uadManager.LoadKPI(now);
            return results;*/
            throw new NotImplementedException();
        }
    }
}