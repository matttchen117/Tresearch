using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Authentication
{
    public class AuthenticationServiceShould : IntegrationTestDependencies
    {
        public AuthenticationServiceShould() : base()
        {
        }

        public void AuthenticateTheUser(IOTPClaim otpClaim)
        {
            // Arrange
            string expected = "success";

            // Act
            List<string> results = authenticationService.Authenticate(otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }

        public void VerifyThatTheUser(IRolePrincipal rolePrincipal)
        {
            // Arrange
            string expected = "success";

            // Act
            string result = authenticationService.VerifyAuthenticated(rolePrincipal);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
