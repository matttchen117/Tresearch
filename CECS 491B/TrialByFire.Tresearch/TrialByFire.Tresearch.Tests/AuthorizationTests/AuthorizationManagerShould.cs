using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Tests.AuthorizationTests
{
    internal class AuthorizationManagerShould
    {
        public void AuthorizeTheUser(IPrincipal _principal)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            IAuthorizationManager _authorizationManager = new AuthorizationManager(_sqlDAO);
            string expected = "success";

            // Act
            string result = _authorizationManager.Authorize(_principal);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
