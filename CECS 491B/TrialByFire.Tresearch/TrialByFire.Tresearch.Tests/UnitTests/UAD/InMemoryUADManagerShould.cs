using System;
namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
	public class InMemoryUADManagerShould
	{
        public void LoadKPI(DateTime now)
        {
            // Arrange
            ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
            ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
            IUADManager _uadManager = new UADManager(_sqlDAO, _logService);
            List<KPI> expected;
            expected.Add(new KPI("success"));

            // Act
            List<KPI> results = _uadManager.LoadKPI(_sqlDAO, _logService, _uadService);

            // Assert
            Assert.Equal(expected, results);
        }

        public void KPISFetched(DateTime now)
        {
            // Arrange
            ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
            ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
            IUADManager _uadManager = new UADManager(_sqlDAO, _logService);
            string expected = "success";

            // Act
            List<KPI> results = _uadManager.KPISFetched(now);

            // Assert
            Assert.Equal(expected, results[0]);
        }
    }
}

