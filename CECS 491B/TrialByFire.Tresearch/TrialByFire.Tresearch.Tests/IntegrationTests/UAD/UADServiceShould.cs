﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.UAD
{
	public class UADServiceShould
	{
		public void LoadKPI(DateTime now)
        {
			// Arrange
			ISqlDAO _sqlDAO = new SqlDAO();
			ILogService _logService = new SqlLogService(_sqlDAO);
			IUADService _uadService = new UADService(_sqlDAO, _logService);
			List<KPI> expected = new List<KPI>();
			expected.Add(new KPI("success"));

			// Act
			List<KPI> results = _uadService.LoadKPI(now);

			// Assert
			Assert.Equal(expected, results);
        }
	}
}
