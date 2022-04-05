using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Tag
{
    public class InMemorySqlDAOShould: TestBaseClass
    {
        public InMemorySqlDAOShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        public async Task AddTagToNodes(List<long> nodeIDs, string tagName, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<InMemorySqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await sqlDAO.AddTagToNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }
    }
}
