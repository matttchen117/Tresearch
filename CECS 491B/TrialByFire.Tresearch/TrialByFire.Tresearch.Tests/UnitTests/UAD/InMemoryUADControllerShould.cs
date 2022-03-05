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
using Xunit;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;

namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
	public class InMemoryUADControllerShould
	{
		public void LoadKPI(DateTime now)
		{
			/*// Arrange
			ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
			ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
			IUADManager _uadManager = new UADManager(_inMemorySqlDAO, _inMemoryLogService);
			UADController _uadController = new UADController(_inMemorySqlDAO, _inMemoryLogService, _uadManager);
			List<KPI> expected = new List<KPI>();
			expected.Add(new KPI("success"));

			// Act
			List<KPI> results = _uadController.LoadKPI(_inMemorySqlDAO, _inMemoryLogService, _uadManager);

			// Assert
			Assert.Equal(expected, results);*/
			throw new NotImplementedException();
		}
	}
}

