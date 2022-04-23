using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.AccountDeletion
{
    public class AccountDeletionServiceShould : TestBaseClass
    {

        public AccountDeletionServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, SqlDAO>();
            TestServices.AddScoped<IAccountDeletionService, AccountDeletionService>();
            TestProvider = TestServices.BuildServiceProvider();

        }

        [Theory]
        [InlineData("altyn@gmail.com", "user", "200: Server: Account Deletion Successful.")]
        [InlineData("ryst@gmail.com", "admin", "200: Server: Account Deletion Successful.")]

        [InlineData("redKeyCard@gmail.com", "admin", "200: Server: Account Deletion Successful.")]

        public async Task DeleteTheUserAsync(string currentIdentity, string currentRole, string expected)
        {


            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            IAccountDeletionService accountDeletionService = TestProvider.GetService<IAccountDeletionService>();

            // Act
            string result = await accountDeletionService.DeleteAccountAsync(cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);

        }

        /*
        public async Task DeleteTheUserAsyncWithin5Seconds(string currentIdentity, string currentRole, string expected)
        {

        }
        */





    }
}