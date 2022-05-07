using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.UnitTests.Recovery
{
    public class InMemoryRecoveryServiceShould: TestBaseClass
    {
        public InMemoryRecoveryServiceShould() : base(new string[] { }) 
        {
            TestServices.AddScoped<IRecoveryService, RecoveryService>();
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }
    }
}
