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
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.AccountDeletion
{
/*    public class AccountDeletionControllerShould
    {

        public void DeleteTheUser(IRolePrincipal principal)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IRolePrincipal _rolePrincipal = new RolePrincipal(principal.Identity);
            IAccountDeletionService _accountDeletionService = new AccountDeletionService(_sqlDAO, _logService, _rolePrincipal);
            string expected = "success";

            // Act
            string result = _accountDeletionController.DeleteAccount(principal);

            // Assert
            Assert.Equal(expected, result);
        }
    }*/
}
