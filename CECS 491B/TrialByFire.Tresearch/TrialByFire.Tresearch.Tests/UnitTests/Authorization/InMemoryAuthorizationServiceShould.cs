using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthorizationTests
{
    public class InMemoryAuthorizationServiceShould
    {
        public void VerifyThatTheUserIsAuthorized(IPrincipal rolePrincipal, string requiredRole)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthorizationService authorizationService = new AuthorizationService(inMemorySqlDAO, inMemoryLogService);
            string expected = "success";

            // Act
            string result = authorizationService.VerifyAuthorized(rolePrincipal, requiredRole);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
