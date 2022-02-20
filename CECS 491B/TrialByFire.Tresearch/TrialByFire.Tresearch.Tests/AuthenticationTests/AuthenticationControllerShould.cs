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
        public void ValidateTheInput(List<string> _input)
        {
            // Triple A Format

            // Arrange
            ISqlDAO _sqlDAO = new ISqlDAO();
            ILogService _logService = new ILogService(_sqlDAO);
            AuthenticationController _authenticationController = new AuthenticationController(_sqlDAO, _logService);
            bool expected = true;

            // Act
            bool result = _authenticationController.ValidateInput(_input);

            // Assert
            Assert.Equal(expected, result);
        }

        public void RequestTheOTP(string _username, string _passphrase)
        {

        }

        public void CreateTheCookie(string _jwtToken)
        {

        }


        public void AuthenticateTheUser(string _username, string _otp)
        {

        }

        public void VerifyTheJWTToken()
        {

        }

        public void CreateThePrincipalObject(string _payload)
        {

        }

        public void VerifyIsAuthenticated()
        {

        }
    }
}
