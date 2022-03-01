using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.IntegrationTests
{
    public class InMemoryAuthenticationServiceShould
    { 

        public void CreateTheJWTToken(string _payload)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAuthenticationService _authenticationService = new SqlAuthenticationService(_sqlDAO, _logService);
            string expected = "success";

            // Act
            List<string> results = _authenticationService.CreateJwtToken(_payload);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void AuthenticateTheUser(IOTPClaim _otpClaim)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAuthenticationService _authenticationService = new SqlAuthenticationService(_sqlDAO, _logService);
            string expected = "success";

            // Act
            List<string> results = _authenticationService.Authenticate(_otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
