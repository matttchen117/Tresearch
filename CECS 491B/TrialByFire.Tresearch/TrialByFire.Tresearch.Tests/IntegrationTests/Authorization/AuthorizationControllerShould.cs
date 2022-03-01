using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthorizationTests
{
    public class AuthorizationControllerShould
    {
        public void AuthorizeTheUser(IPrincipal _principal)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            ILogService _logService;
            IAuthorizationController _authorizationController = new AuthorizationController(_sqlDAO, _logService);
            string expected = "success";

            // Act
            string result = _authorizationController.Authorize(_principal);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
