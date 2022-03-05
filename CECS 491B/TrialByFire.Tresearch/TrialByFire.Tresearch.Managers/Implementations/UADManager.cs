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
		private ISqlDAO _sqlDAO { get; }
		private ILogService _logService { get; }

		private IUADService _uadService { get; }
		private IAuthenticationService _authenticationService { get; }
		private IAuthorizationService _authorizationService { get; }
		
		private IRolePrincipal _rolePrincipal { get; }
		private IOTPRequestService _otpRequestService { get; }
		private readonly string _role = "Admin";

		public UADManager(ISqlDAO sqlDAO, ILogService logService, IUADService uadService, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
        {
			_sqlDAO = sqlDAO;
			_logService = logService;
			_uadService = uadService;
			_authenticationService = authenticationService;
			_authorizationService = authorizationService;
        }

		public List<KPI> LoadKPI(DateTime now)
        {
			//return _uadService.LoadKPI(now);
			throw new NotImplementedException();
        }

		public List<KPI> KPIsFetched(DateTime now)
        {
			string result;
			result = _authenticationService.VerifyAuthenticated(_rolePrincipal);
			if(result == "success")
            {
				string authorizeResult;
				authorizeResult = _authorizationService.VerifyAuthorized(_rolePrincipal, _role);
				if (authorizeResult == "success")
                {
					Task t1 = Task.Run(() =>
					{
						return _uadService.LoadKPI(now);
					});

                    if (!t1.Wait(60000))
                    {
						List<KPI> results = new List<KPI>();
						KPI failureKPI = new KPI("Error 504; Timeout Error");
						results.Add(failureKPI);
						return results;
                    }
                }
            }
			List<KPI> resultList = new List<KPI>();
			resultList.Add(new KPI("Error: Timeout"));
			return resultList;
        }
    }
}

