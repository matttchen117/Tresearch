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
using TrialByFire.Tresearch.Services.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.AccountDeletion
{
/*    public class AccountDeletionManagerShould
    {
        public void DeleteTheUser(IRolePrincipal principal)
        {

            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAccountDeletionService _accountDeletionService = new AccountDeletionService();
            IAccountDeletionManager _accountDeletionManager = new AccountDeletionManager();
            string expected = "success";
            
            // Act
            string _result = _accountDeletionManager.DeleteAccount(principal);

            // Assert
            Assert.Equal(expected, _result);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking
        }
    }*/
}
