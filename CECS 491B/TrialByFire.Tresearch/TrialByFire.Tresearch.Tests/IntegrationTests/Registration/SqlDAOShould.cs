using Xunit;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using Microsoft.Extensions.DependencyInjection;


namespace TrialByFire.Tresearch.Tests.IntegrationTests.Registration
{
    public class SqlDAOShould: TestBaseClass
    {
        public SqlDAOShould() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData(1, "0c73979d72f9243294fe095e09cae5eb4c9eca10a4f35c648c5301433341358b5c8e3b595af32f3a08e94ece868401d5d26b99c0681810c4d89fd51a95314953",  "200: Server: success")]            // Hash doesn't exist
        [InlineData(2, "db4c939b4b5feab3194957cdce046084d2c6fec58b3474db02c2b175b715db1ce6536f133b9ffb961b9d8251bf82084ca5fadb5d1daa4cb792860a394aa38e15", "200: Server: success")]     // Hash already exists, Account was previously deleted
        public async Task CreateUserHashAsync(int id, string hashedEmail, string status)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            string result = await sqlDAO.CreateUserHashAsync(id, hashedEmail, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(status, result);
        }

        [Theory]
        [MemberData(nameof(AccountInfo))]
        public async Task CreateAccountAsync(IAccount account, string expected)
        {
            //Arrange
            ISqlDAO sqlDAO = TestProvider.GetService<ISqlDAO>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            //Act
            Tuple<int, string> result = await sqlDAO.CreateAccountAsync(account, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result.Item2);
        }

        public static IEnumerable<object[]> AccountInfo()
        {
            // Account has hash, success
            IAccount account1 = new Account("sqlDAORegTest1@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected1 = "200: Server: success";

            //Account has hash, Already has account
            IAccount account2 = new Account("sqlDAORegTest2@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected2 = "409: Server: Account  already exists";

            return new[]
            {
                new object[] { account1, expected1  },
                new object[] { account2, expected2  }
            };
        }

        public async Task CreateConfirmationLink(string email, string authorizationlevel, string expected)
        {

        }
    }
}
