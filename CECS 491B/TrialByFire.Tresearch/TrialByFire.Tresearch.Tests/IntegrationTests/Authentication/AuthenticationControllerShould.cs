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
using XUnit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.IntegrationTests
{
    public class InMemoryAuthenticationControllerShould
    {

        public void AuthenticateTheUser(string _username, string _otp)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAuthenticationManager _authenticationManager = new AuthenticationManager(_sqlDAO, _logService);
            AuthenticationController _authenticationController = new AuthenticationController(_sqlDAO, _logService, _authenticationManager);
            string expected = "success";

            // Act
            List<string> results = _authenticationController.Authenticate(_username, _otp, DateTime.Now);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void CreateTheCookie(string _jwtToken)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAuthenticationManager _authenticationManager = new AuthenticationManager(_sqlDAO, _logService);
            AuthenticationController _authenticationController = new AuthenticationController(_sqlDAO, _logService, _authenticationManager);
            string expected = "success";

            // Act
            string result = _authenticationController.CreateCookie(_jwtToken);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
