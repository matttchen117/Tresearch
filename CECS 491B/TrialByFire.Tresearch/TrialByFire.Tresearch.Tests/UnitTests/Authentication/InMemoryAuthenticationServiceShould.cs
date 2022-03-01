using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implentations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationServiceShould
    { 
        
        public void CreateTheJWTToken(string _payload)
        {
            // Arrange
            IAuthenticationService _inMemoryAuthenticationService = new InMemoryAuthenticationService();
            string expected = "success";

            // Act
            List<string> results = _inMemoryAuthenticationService.CreateJwtToken(_payload);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void AuthenticateTheUser(IOTPClaim _otpClaim)
        {
            // Arrange
            IAuthenticationService _inMemoryAuthenticationService = new InMemoryAuthenticationService();
            string expected = "success";

            // Act
            List<string> results = _inMemoryAuthenticationService.Authenticate(_otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
