using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    public class AuthenticationServiceShould
    { 
        public void AuthenticateTheUser(IOTPClaim otpClaim)
        {
            // Arrange
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(sqlDAO, logService);
            string expected = "success";

            // Act
            List<string> results = authenticationService.Authenticate(otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }

        public void VerifyThatTheUser(IRolePrincipal rolePrincipal)
        {
            // Arrange
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(sqlDAO, logService);
            string expected = "success";

            // Act
            string result = authenticationService.VerifyAuthenticated(rolePrincipal);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
