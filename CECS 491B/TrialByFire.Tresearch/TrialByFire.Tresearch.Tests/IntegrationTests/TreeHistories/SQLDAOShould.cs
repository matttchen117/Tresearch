using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.TreeHistories
{
    public class SQLDAOShould : TestBaseClass
    {
        public SQLDAOShould() : base (new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
        }


        [Theory]
        [MemberData(nameof(CreateTreeHistoryData))]
        public async Task CreateTreeHistoryAsync() { 
        
        }

        [Theory]
        [MemberData(nameof(GetTreeHistoryData))]
        public async Task GetTreeHistoryAsync()
        {

        }

        public IEnumerable CreateTreeHistoryData() { 
        
        }

        public IEnumerable GetTreeHistoryData() { 
        
        }
    }
}
