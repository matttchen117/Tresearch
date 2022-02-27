using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.IntegrationTests
{
    public class AuthenticationServiceShould
    { 

        public void CreateTheJWTToken(string _payload)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            IAuthenticationService _authenticationService = new AuthenticationService(_sqlDAO);
            string expected = "success";

            // Act
            List<string> results = _authenticationService.CreateJwtToken(_payload);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void AuthenticateTheUser(IOTPClaim _otpClaim)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            IAuthenticationService _authenticationService = new AuthenticationService(_sqlDAO);
            string expected = "success";

            // Act
            List<string> results = _authenticationService.Authenticate(_otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
