using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.UAD
{
    public class InMemoryUADManagerShould : InMemoryTestDependencies
    {
        public InMemoryUADManagerShould() : base()
        {
        }

        [Theory]
        [InlineData(2022, 3, 7, "success")]
        [InlineData(2021, 12, 12, "Error")]
        public async Task LoadKPIs(int year, int month, int day, string expected)
        {
            // Arrange
            IUADService uadService = new UADService(SqlDAO, LogService);
            IAuthenticationService authenticationService = new AuthenticationService(SqlDAO, LogService, MessageBank);
            IAuthorizationService authorizationService = new AuthorizationService(SqlDAO, LogService);
            IUADManager uadManager = new UADManager(SqlDAO, LogService, uadService, authenticationService, authorizationService, MessageBank);
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            // Act
            List<IKPI> results = new List<IKPI>();
            results = await uadManager.LoadKPIAsync(new DateTime(year, month, day));

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