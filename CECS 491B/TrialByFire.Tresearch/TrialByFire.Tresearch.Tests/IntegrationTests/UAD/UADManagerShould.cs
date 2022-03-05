using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.UAD
{
	public class UADManagerShould
	{
		public void LoadKPI(DateTime now)
        {
            /*// Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IUADManager _uadManager = new UADManager(_sqlDAO, _logService);
            List<KPI> expected;
            expected.Add(new KPI("success"));

            // Act
            List<KPI> results = _uadManager.LoadKPI(_sqlDAO, _logService, _uadService);

            // Assert
            Assert.Equal(expected, results);*/
        }

        public void KPISFetched(DateTime now)
        {

        }
	}
}

