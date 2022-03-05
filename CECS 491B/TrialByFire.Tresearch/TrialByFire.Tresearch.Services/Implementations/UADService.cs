using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TrialByFire.Tresearch.Models.Implementations;

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

		public List<KPI> LoadKPI(DateTime now)
		{
			throw new NotImplementedException();
			//return _sqlDAO.LoadKPI(now);
		}
	}
}

