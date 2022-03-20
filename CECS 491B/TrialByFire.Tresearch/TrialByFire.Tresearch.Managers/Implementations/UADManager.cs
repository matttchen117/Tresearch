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
		private IMessageBank _messageBank { get; }
		//private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);


		public UADManager(ISqlDAO sqlDAO, ILogService logService, IUADService uadService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IMessageBank messageaBank)
		{
			_sqlDAO = sqlDAO;
			_logService = logService;
			_uadService = uadService;
			_authenticationService = authenticationService;
			_authorizationService = authorizationService;
			_messageBank = messageaBank;
		}

		public async Task<List<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			List<IKPI> result = new List<IKPI>();
			if (Thread.CurrentPrincipal != null)
            {

				try
				{
					result = await _uadService.LoadKPIAsync(now, cancellationToken).ConfigureAwait(false);
					return result;
				}
				catch (Exception ex)
				{
					result.Add(new KPI("400: Server: LoadKPI Error Occurred"));
					return result;
				}
			}
			result.Add(new KPI(_messageBank.ErrorMessages["notAuthenticated"]));
			return result;
		}
	}
}
