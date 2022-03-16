using System;
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

namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
	public class InMemoryUADServiceShould : InMemoryTestDependencies
	{
		public InMemoryUADServiceShould() : base()
		{
		}

		[Theory]
		[InlineData(2022, 3, 6, "success")]
		[InlineData(2021, 1, 1, "Error")]
		public void LoadKPI(int year, int month, int day, string expected)
		{
			// Arrange
			IUADService uadService = new UADService(SqlDAO, LogService);

			// Act
			List<IKPI> results = new List<IKPI>();
			results = uadService.LoadKPI(new DateTime(year, month, day));

			// Assert
			string ex = "success";
			foreach (var x in results)
			{
				if (x.result != "success")
				{
					ex = "Error";
				}
			}
			Assert.Equal(expected, ex);
		}
	}
}

