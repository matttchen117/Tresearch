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

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationServiceShould
    { 
        
        public void CreateTheJWTToken(string _payload)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(inMemorySqlDAO, authenticationService);
            string expected = "success";

            // Act
            List<string> results = authenticationService.CreateJwtToken(_payload);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void AuthenticateTheUser(IOTPClaim _otpClaim)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(inMemorySqlDAO, authenticationService);
            string expected = "success";

            // Act
            List<string> results = authenticationService.Authenticate(_otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
