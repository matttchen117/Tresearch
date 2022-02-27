using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests
{
    internal class AuthenticationManagerShould
    {

        public void CreateTheOTPClaim(_username, _otp, now)
        {
            // Arrange

            // Act
            IOTPClaim _otpClaim = new OTPClaim(_username, _otp, now);

            // Assert
            Assert.NotNull(_otpClaim);

            //Unnecessary
        }

        public void AuthenticateTheUser(string _username, string _otp, DateTime now)
        {
            // Arrange
            ISqlDAO _sqlDAO;
            IAuthenticationManager _authenticationManager = new AuthenticationManager(_sqlDAO);
            string expected = "success";

            // Act
            List<string> results = _authenticationManager.Authenticate(_username, _otp, now);

            // Assert
            Assert.Equal(expected, results[0]);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking
        }

    }
}
