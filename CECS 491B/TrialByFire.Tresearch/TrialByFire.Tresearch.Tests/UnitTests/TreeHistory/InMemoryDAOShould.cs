using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.TreeHistory
{
    public class InMemoryDAOShould : TestBaseClass
    {
        private ISqlDAO _sqldao;
        private InMemoryDatabase _inMemoryDatabase;
        public InMemoryDAOShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
            _sqldao = TestProvider.GetService<ISqlDAO>();
            _inMemoryDatabase = new InMemoryDatabase();
        }

        [Theory]
        [MemberData(nameof(CreateTreeHistoryData))]
        public async Task CreateTreeHistory()
        {
            // Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // Act
            // Assert
        }

        [Theory]
        [MemberData(nameof(GetTreeHistoryData))]
        public async Task GetTreeHistory()
        {
            // Arrange
            // Act
            // Assert

        }

        public static IEnumerable<object[]> CreateTreeHistoryData()
        {
            /**
            *   Case 1: Tree history created. It does not exist in database. 
            *   
            */

            /**
            *   Case 2: Tree history already exists. 
            */
        }

        public static IEnumerable<object[]> GetTreeHistoryData()
        {
            /*
            *   Case 1: Tree history successfully retrieved. Exists in Database. 
            */
            
            /**
            *   Case 2: Tree history does not exist in database 
            */
        }
    }
}
