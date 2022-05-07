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
        [MemberData(nameof(CreateTreeHistoryAuditData))]
        public async Task CreateTreeHistoryAudit(List<INodeHistory> nodeHistories, DateTime creationTime, int versionNumber, long rootNodeID,  IMessageBank.Responses response)
        {
            // Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            // Act
            string result = await sqlDAO.CreateTreeHistoryAsync(nodeHistories, creationTime, versionNumber, rootNodeID, response);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTreeHistoryAuditData))]
        public async Task GetTreeHistoryAudit()
        {
            // Arrange
            // Act
            // Assert

        }

        public static IEnumerable<object[]> CreateTreeHistoryAuditData()
        {
            /**
            *   Case 1: Tree history created. It does not exist in database. 
            *   
            */
            var versionNodeHistories1 = new List<INodeHistory>();
            var versionCreationDate1 = new DateTime(2022, 6, 6, 0, 5, 19);
            var versionNumber1 = 4;
            var versionRootNodeID1 = 1;
            var response1 = IMessageBank.Responses.treeHistoryCreatedSuccess;

            /**
            *   Case 2: Tree history already exists. 
            */
            var versionNodeHistories2 = new List<INodeHistory>();
            var versionCreationDate2 = new DateTime(2022, 5, 3, 0, 5, 19);
            var versionNumber2 = 1;
            var versionRootNodeID2 = 1;
            var response2 = IMessageBank.Responses.treeHistoryDuplicate;

            return new[]
            {
                new object[]{versionNodeHistories1, versionCreationDate1, versionNumber1, versionRootNodeID1, response1},
                new object[]{versionNodeHistories2, versionCreationDate2, versionNumber2, versionRootNodeID2, response2}
            };
        }

        public static IEnumerable<object[]> GetTreeHistoryAuditData()
        {
            /*
            *   Case 1: Tree history successfully retrieved. Exists in Database. 
            */

            var versionNumber1 = 1;
            var versionCreationDate1 = new DateTime(2022, 5, 3, 0, 19, 30);
            var versionRootNodeID1 = 1;
            var response1 = IMessageBank.Responses.treeHistoryGetSuccess;

            return new []
            {
                new object[]{versionNumber1, versionCreationDate1, versionRootNodeID1, response1}
            };
        }
    }
}
