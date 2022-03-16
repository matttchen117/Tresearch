using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
	public class UADControllerShould : IntegrationTestDependencies
	{
		public UADControllerShould() : base()
		{
		}

		[Theory]
		[InlineData(2022, 3, 5, "success")]
		[InlineData(2021, 12, 12, "Error")]
		public void LoadKPI(int year, int month, int day, string expected)
		{
			// Arrange
			IUADService uadService = new UADService(SqlDAO, SqlLogService);
			IUADManager uadManager = new UADManager(SqlDAO, SqlLogService, uadService, AuthenticationService, AuthorizationService);
			IUADController uadController = new UADController(SqlDAO, SqlLogService, uadManager);

			// Act
			List<IKPI> results = new List<IKPI>();
			results = uadController.LoadKPI(new DateTime(year, month, day));

			// Assert
			Assert.Equal(expected, results[0].result);
		}
	}
}

