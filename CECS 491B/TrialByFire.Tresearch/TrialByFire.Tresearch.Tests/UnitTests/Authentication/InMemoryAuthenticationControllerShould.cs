using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationControllerShould
    {

        public void AuthenticateTheUser(string username, string otp)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthenticationManager authenticationManager = new AuthenticationManager(inMemorySqlDAO, inMemoryLogService);
            IAuthenticationController authenticationController = new AuthenticationController(inMemorySqlDAO, inMemoryLogService, authenticationManager);
            string expected = "success";

            // Act
            string result = authenticationController.Authenticate(username, otp);

            // Assert
            Assert.Equal(expected, result);
        }

        public void CreateTheCookie(string username, string otp)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthenticationManager authenticationManager = new AuthenticationManager(inMemorySqlDAO, inMemoryLogService);
            IAuthenticationController authenticationController = new AuthenticationController(inMemorySqlDAO, inMemoryLogService, authenticationManager);
            string _jwtToken = authenticationManager.Authenticate(username, otp, DateTime.Now)[1];
            string expected = "success";

            // Act
            string result = authenticationController.CreateCookie(_jwtToken);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
