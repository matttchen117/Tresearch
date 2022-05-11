using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
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

		public async Task<IResponse<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellationToken = default)
		{
			List<IKPI> result = new List<IKPI>();
			if ((Thread.CurrentPrincipal.Identity as RoleIdentity).AuthorizationLevel != "admin")
			{
				IResponse<IKPI> response = await _uadService.LoadKPIAsync(now, cancellationToken).ConfigureAwait(false);

				if (cancellationToken.IsCancellationRequested)
				{
					MethodBase? m = MethodBase.GetCurrentMethod();
					if (m != null)
					{
						response.ErrorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationTimeExceeded).ConfigureAwait(false);
					}
				}

				return response;
			}
            else
            {
				return new UADResponse<IKPI>(await _messageBank.GetMessage(
					IMessageBank.Responses.notAuthorized).ConfigureAwait(false), null, 400, false);
			}
		}
	}
}
