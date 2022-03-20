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
	public class InMemoryUADControllerShould : InMemoryTestDependencies
	{
		public InMemoryUADControllerShould() : base()
		{
		}

		[Theory]
		[InlineData(2022, 3, 7, "success")]
		[InlineData(2021, 1, 1, "Error")]
		public async Task LoadKPI(int year, int month, int day, string expected)
		{
			// Arrange
			IMessageBank messageBank = new MessageBank();
			IAuthenticationService authenticationService = new AuthenticationService(SqlDAO, LogService, messageBank);
			IAuthorizationService authorizationService = new AuthorizationService(SqlDAO, LogService);
			IUADService uadService = new UADService(SqlDAO, LogService);
			IUADManager uadManager = new UADManager(SqlDAO, LogService, uadService, authenticationService, authorizationService);
			IUADController uadController = new UADController(SqlDAO, LogService, uadManager);

			// Act
			List<IKPI> results = new List<IKPI>();
			results = await uadController.LoadKPIAsync(new DateTime(year, month, day));

			// Assert
			string ex = "success";
			foreach (var x in results)
			{
				if (x.result != "Success")
				{
					ex = "Error";
				}
			}
			Assert.Equal(expected, ex);
		}
	}
}
