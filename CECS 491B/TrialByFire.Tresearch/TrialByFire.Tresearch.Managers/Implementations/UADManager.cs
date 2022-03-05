using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
	public class UADManager : IUADManager
	{
		private readonly ISqlDAO _sqlDAO;
		private readonly ILogService _logService;
		private readonly IUADService _uadService;

		public UADManager(ISqlDAO sqlDAO, ILogService logService, IUADService uadService)
        {
			_sqlDAO = sqlDAO;
			_logService = logService;
			_uadService = uadService;
        }

		public List<KPI> LoadKPI(DateTime now)
        {
			return _uadService.LoadKPI(now);
        }

		public List<KPI> KPISFetched(DateTime now)
        {
			Task t1 = Task.Run(() =>
			{
				 return _uadService.LoadKPI(now);
			});
            if (!t1.Wait(60000))
            {
				List<KPI> result = new List<KPI>();
				result.Add(new KPI("Error 504: Timeout Error"));
				return result;
            }
			throw new NotImplementedException();
		}
        public bool KPISFetched()
        {
            throw new NotImplementedException();
        }
    }
}

