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
            // UserAccount has hash, success
            IAccount account1 = new UserAccount("sqlDAORegTest1@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected1 = "200: Server: success";

            //UserAccount has hash, Already has account
            IAccount account2 = new UserAccount("sqlDAORegTest2@gmail.com", "1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8", "user", true, false);
            string expected2 = "409: Server: UserAccount  already exists";

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
