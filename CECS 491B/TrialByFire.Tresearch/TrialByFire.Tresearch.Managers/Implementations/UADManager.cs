using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
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
		private readonly string _authorizationLevel = "Admin";
		private static CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
		private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);


		public UADManager(ISqlDAO sqlDAO, ILogService logService, IUADService uadService, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
		{
			_sqlDAO = sqlDAO;
			_logService = logService;
			_uadService = uadService;
			_authenticationService = authenticationService;
			_authorizationService = authorizationService;
		}

		public async Task<List<IKPI>> LoadKPIAsync(DateTime now)
		{
			List<IKPI> result = new List<IKPI>();			
			await _semaphoreSlim.WaitAsync();
			try
			{
				CheckPermissions();
				result = await _uadService.LoadKPIAsync(now, _cts.Token).ConfigureAwait(false);
			}
			catch (TaskCanceledException tcex)
			{
				
			}
			catch (SecurityException se)
			{

			}
			catch (Exception ex)
            {

            }
            finally
            {
				_semaphoreSlim.Release();
            }
			return result;
		}

		private void CheckPermissions()
		{
			string authorizeResult = _authorizationService.VerifyAuthorized(_rolePrincipal, _authorizationLevel);
			if (authorizeResult != "success")
			{
				throw new SecurityException("User Not Allowed to LoadKPIs");
			}
		}

		/*public List<KPI> KPIsFetched(DateTime now)
		{
			string result;
			string authorizeResult;
			authorizeResult = _authorizationService.VerifyAuthorized(_rolePrincipal, _authorizationLevel);
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
			List<KPI> resultList = new List<KPI>();
			resultList.Add(new KPI("Error: Timeout"));
			return resultList;
		}*/
	}
}
