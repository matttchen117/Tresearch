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
        [InlineData("altyn@gmail.com", "user",
            "73CCCB45EF608E4E6D1C08E8AD1680EDD506E358989FE29ECF2444737AFAB1BCAB4AF3F018E27E892EEB50090E5A9297DD8AC4D94E661680359EC4AF2ED99075",
            "success")]
        [InlineData("ryst@gmail.com", "admin",
            "2BAE41CCA36995BD81EE7C78E2D2791351883C5296D120E2867B035E8548D4F0A48E31AACD423CF4C70A1A945BB9E00E8911AC8F0BE7CC5415D7C38EAF092E6B",
            "success")]
        [InlineData("redKeyCard@gmail.com", "admin",
            "7010245ED3B2F9174B51AA18559DA1B4E6F676BD3D2ED504F65E4CFE19C757CBD605139DFFF442472D079E5F2C943676E8768C839083D4CEED4783D1BEA6FE34",
            "Database: The account was not found.")]
        public void DeleteTheUser(string currentIdentity, string currentRole, string userHash, 
            string expected)
        {

            
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole, userHash);
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
