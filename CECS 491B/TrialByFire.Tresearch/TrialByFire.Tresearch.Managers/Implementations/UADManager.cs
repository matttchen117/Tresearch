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
		private CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
		//private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);


		public UADManager(ISqlDAO sqlDAO, ILogService logService, IUADService uadService, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
		{
			_sqlDAO = sqlDAO;
			_logService = logService;
			_uadService = uadService;
			_authenticationService = authenticationService;
			_authorizationService = authorizationService;
		}

		public async Task<List<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			List<IKPI> result = new List<IKPI>();		
			//await _semaphoreSlim.WaitAsync();
			try
			{
				//CheckPermissions();
				result = await _uadService.LoadKPIAsync(now, _cts.Token);
			}
			catch (TaskCanceledException tcex)
			{
				result.Add(new KPI(tcex.Message));
			}
			catch (SecurityException se)
			{
				result.Add(new KPI(se.Message));
			}
			catch (Exception ex)
            {
				result.Add(new KPI(ex.Message));
            }
			return result;
		}

		private void CheckPermissions()
		{
			string authorizeResult = _authorizationService.VerifyAuthorized(_rolePrincipal, _authorizationLevel);
			if (authorizeResult != "Success")
			{
				throw new SecurityException("User Not Allowed to LoadKPIs");
			}
		}
	}
}
