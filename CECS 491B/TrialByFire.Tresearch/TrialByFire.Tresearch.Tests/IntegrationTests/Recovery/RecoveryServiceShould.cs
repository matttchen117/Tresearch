using Xunit;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Recovery
{
    public class RecoveryServiceShould: IntegrationTestDependencies
    {
        public RecoveryServiceShould() : base() { }

        [Theory]
        [InlineData("IntegrationRecoveryService1@gmail.com","32F7D767-2466-4C96-A527-5D4FA9612C09", "200: Server: success")]  
        [InlineData("IntegrationRecoveryService2@gmail.com", "42f73767-2461-4c96-a52g-5d4fa9612c09", "200: Server: success")]  
        public async Task GetRecoveryLinkInfo(string username, string guid, string statusCode) 
        {
            //Arrange
            string expected = username;
            string expectedStatusCode = statusCode;

            //Act
            Tuple<IRecoveryLink, string> results = await RecoveryService.GetRecoveryLinkAsync(guid).ConfigureAwait(false);
            string result = results.Item1.Username;
            string resultStatusCode = results.Item2;

            //Assert
            Assert.Equal(expected, result);
            Assert.Equal(expectedStatusCode, resultStatusCode);
            
        }

    }

    
}
