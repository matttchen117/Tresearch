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
    public class AccountDeletionServiceShould : IntegrationTestDependencies
    {

        /*
        public AccountDeletionServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("altyn@gmail.com", "user", "success")]
        [InlineData("ryst@gmail.com", "admin", "success")]
        [InlineData("redKeyCard@gmail.com", "admin", "Database: The account was not found.")]




        public void DeleteTheUser(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IAccountDeletionService accountDeletionService = new AccountDeletionService(SqlDAO, LogService);

            // Act
            string result = accountDeletionService.DeleteAccount();

            // Assert
            Assert.Equal(expected, result);
        }

        */



    }
}
