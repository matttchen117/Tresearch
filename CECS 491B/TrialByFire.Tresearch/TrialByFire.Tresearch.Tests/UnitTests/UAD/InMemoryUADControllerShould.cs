using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers;
using XUnit;

namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
	public class InMemoryUADControllerShould
	{
		public void LoadKPI(DateTime now)
		{
			// Arrange
			ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
			ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
			IUADManager _uadManager = new UADManager(_inMemorySqlDAO, _inMemoryLogService);
			UADController _uadController = new UADController(_sqlDAO, _logService, _uadManager);
			List<KPI> expected;
			expected.Add(new KPI("success"));

			// Act
			List<KPI> results = _uadController.LoadKPI(_sqlDAO, _logService, _uadManager);

			// Assert
			Assert.Equal(expected, results);
		}
	}
}

