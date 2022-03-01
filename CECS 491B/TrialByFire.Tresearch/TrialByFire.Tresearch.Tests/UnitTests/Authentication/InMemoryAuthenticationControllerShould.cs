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
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationControllerShould
    {

        public void AuthenticateTheUser(string _username, string _otp)
        {
            // Arrange
            ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
            ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
            IAuthenticationManager _inMemoryAuthenticationManager = new InMemoryAuthenticationManager();
            AuthenticationController _authenticationController = new AuthenticationController(_inMemorySqlDAO, _inMemoryLogService, _inMemoryAuthenticationManager);
            string expected = "success";

            // Act
            IEnumerable<string> results = _authenticationController.Authenticate(_username, _otp);

            // Assert
            Assert.Equal(expected, results.ElementAt(0));
        }

        public void CreateTheCookie(string username, string otp)
        {
            // Arrange
            ISqlDAO _inMemorySqlDAO = new InMemorySqlDAO();
            ILogService _inMemoryLogService = new InMemoryLogService(_inMemorySqlDAO);
            IAuthenticationManager _inMemoryAuthenticationManager = new InMemoryAuthenticationManager();
            AuthenticationController _authenticationController = new AuthenticationController(_inMemorySqlDAO, _inMemoryLogService, _inMemoryAuthenticationManager);
            string _jwtToken = _inMemoryAuthenticationManager.Authenticate(username, otp, DateTime.Now)[1];
            string expected = "success";

            // Act
            string result = _authenticationController.CreateCookie(_jwtToken);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
