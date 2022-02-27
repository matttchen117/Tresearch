using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests
{
    internal class AuthenticationControllerShould
    {

        public void AuthenticateTheUser(string _username, string _otp)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            ILogService _logService;
            IAuthenticationController _authenticationController = new AuthenticationController(_sqlDAO, _logService);
            string expected = "success";

            // Act
            List<string> results = _authenticationController.Authenticate(_username, _otp, DateTime.Now);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        public void CreateTheCookie(string _jwtToken)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            ILogService _logService;
            IAuthenticationController _authenticationController = new AuthenticationController(_sqlDAO, _logService);
            string expected = "success";

            // Act
            string result = _authenticationController.CreateCookie(_jwtToken);

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
