using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
	public class InMemoryUADControllerShould : TestBaseClass
	{
		public InMemoryUADControllerShould() : base(new string[] { })
		{
			TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
			TestServices.AddScoped<IUADService, UADService>();
			TestServices.AddScoped<IUADManager, UADManager>();
			TestServices.AddScoped<IUADController, UADController>();
			TestProvider = TestServices.BuildServiceProvider();
		}

		/*[Theory]
		[InlineData(2022, 3, 7, "success")]
		[InlineData(2021, 1, 1, "Error")]
		public async Task LoadKPI(int year, int month, int day, string expected)
		{
			// Arrange
			IUADController uadController = TestProvider.GetService<IUADController>();

			// Act
			List<IKPI> results = new List<IKPI>();
			IResponse<IKPI> result = await uadController.LoadKPIAsync(new DateTime(year, month, day));

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
		}*/
	}
}
