using System;
using System.Collections.Generic;
using System.Linq;
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
    public class InMemoryAccountDeletionManagerShould : InMemoryTestDependencies
    {
        public InMemoryAccountDeletionManagerShould() : base()
        {
        }

        [Theory]
        [InlineData("trizip@gmail.com", "user", "success")]
        [InlineData("switchblade@gmail.com", "admin", "success")]
        [InlineData("greenKeyCard@gmail.com", "user", "Database: The account was not found.")]

        public void DeleteTheUser(string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IAccountDeletionManager accountDeletionManager = new AccountDeletionManager(sqlDAO, logService, accountDeletionService, rolePrincipal);

            // Act
            string result = accountDeletionManager.DeleteAccount(rolePrincipal);

            // Assert
            Assert.Equal(expected, result);

        }


    }
}
