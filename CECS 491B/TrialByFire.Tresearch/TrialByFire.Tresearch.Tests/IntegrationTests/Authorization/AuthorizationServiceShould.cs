using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthorizationTests
{
    public class AuthorizationServiceShould
    {
        public void AuthorizeTheUser(IPrincipal _principal)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            IAuthorizationService _authorizationService = new AuthorizationService(_sqlDAO);
            string expected = "success";

            // Act
            string result = _authorizationService.Authorize(_principal);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
