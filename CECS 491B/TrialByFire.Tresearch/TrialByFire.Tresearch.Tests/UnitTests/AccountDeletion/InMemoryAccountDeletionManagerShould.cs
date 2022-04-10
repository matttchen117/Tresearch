using Microsoft.AspNetCore.Mvc;
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

namespace TrialByFire.Tresearch.Tests.UnitTests.AccountDeletion
{
    public class InMemoryAccountDeletionManagerShould : TestBaseClass
    {
        public InMemoryAccountDeletionManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IAccountDeletionService, AccountDeletionService>();
            TestServices.AddScoped<IAccountDeletionManager, AccountDeletionManager>();
            TestProvider = TestServices.BuildServiceProvider();

        }

        [Theory]
        [InlineData("trizip@gmail.com", "user",
                    "278DB29E1AD1C3E5EE52FEEFA738BC20EE32AF5B33EE0C25F5B73EEE3599839771381A750D3F67E14C14978208559509C83D60D4F425ACE728EBB8305875138F",
                    "success")]
        [InlineData("switchblade@gmail.com", "admin",
                    "E4AB115F44C7D8C9D5F2465043AC9F02DC1432697273A08400C2C8FACDD94CCE6FA69663AE44463378B79588F7EE178E1E98C16161E485F74DDC139DAEF1BCED",
                    "success")]
        [InlineData("greenKeyCard@gmail.com", "user",
                    "63855EDCC64EB97A041304FC59FCC649694F640CF11D4F33A2395917E89085F5E027697A00E456839B2B879A69EB3738C8501BE0D43D4D8A0DFF7AE03CD12E23",
                    "Database: The account was not found.")]

        public void DeleteTheUser(string currentIdentity, string currentRole, string userHash, 
            string expected)
        {

            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole, userHash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }


            IAccountDeletionManager accountDeletionManager = TestProvider.GetService<IAccountDeletionManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));


            // Act
            string result = await accountDeletionManager.DeleteAccountAsync(cancellationTokenSource.Token).ConfigureAwait(false);


            // Assert
            Assert.Equal(expected, result);



        }


    }
}
