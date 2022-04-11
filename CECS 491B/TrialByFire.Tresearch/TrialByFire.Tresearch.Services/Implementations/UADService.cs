using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TrialByFire.Tresearch.Services.Implementations
{
	public class UADService : IUADService
	{
		public ISqlDAO _sqlDAO { get; set; }
		public ILogService _logService { get; set; }

		public UADService(ISqlDAO _sqlDAO, ILogService _logService)
		{
			this._sqlDAO = _sqlDAO;
			this._logService = _logService;
		}

		public async Task<List<IKPI>> LoadKPIAsync(DateTime now, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			List<IKPI> kpiList = new List<IKPI>();
            try
            {
				kpiList.Add(await _sqlDAO.GetViewKPIAsync(cancellationToken).ConfigureAwait(false));
				kpiList.Add(await _sqlDAO.GetViewDurationKPIAsync(cancellationToken).ConfigureAwait(false));
				kpiList.Add(await _sqlDAO.GetNodeKPIAsync(now, cancellationToken).ConfigureAwait(false));
				kpiList.Add(await _sqlDAO.GetSearchKPIAsync(now, cancellationToken).ConfigureAwait(false));
				kpiList.Add(await _sqlDAO.GetLoginKPIAsync(now, cancellationToken).ConfigureAwait(false));
				kpiList.Add(await _sqlDAO.GetRegistrationKPIAsync(now, cancellationToken).ConfigureAwait(false));
				return kpiList;
            }
            catch(TaskCanceledException tcex)
            {
				kpiList.Add(new KPI(tcex.Message));
				return kpiList;
            }
            catch(Exception ex)
            {
				kpiList.Add(new KPI(ex.Message));
				return kpiList;
            }
		}
	}
}

