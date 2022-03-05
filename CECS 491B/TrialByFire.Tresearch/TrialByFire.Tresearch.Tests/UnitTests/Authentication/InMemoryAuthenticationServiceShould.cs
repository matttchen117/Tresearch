using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationServiceShould : InMemoryTestDependencies
    { 
        public InMemoryAuthenticationServiceShould() : base()
        {
        }

        /*        public void CreateTheJWTToken(string _payload)
                {
                    // Arrange
                    string expected = "success";

                    // Act
                    List<string> results = authenticationService.CreateJwtToken(_payload);

                    // Assert
                    Assert.Equal(expected, results[0]);
                }*/

        [Theory]
        [InlineData("larry@gmail.com", "ABCdef123", 2022, 3, 4, 5, 6, 0, "success")]
        [InlineData("billy@yahoo.com", "abcdef123", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "abcdefghi", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("joe@outlook.com", "ABCdef123", 2023, 3, 4, 5, 6, 0, "success")]
        [InlineData("bob@yahoo.com", "ABCdef123", 2022, 3, 4, 5, 6, 0, "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("harry@yahoo.com", "ABCdef123", 2022, 3, 4, 5, 6, 0, "Database: Please click on the confirmation " +
            "link that we sent to your email in order to confirm your account.")]
        public void AuthenticateTheUser(string username, string otp, int year, int month, int day, int hour,
            int minute, int second, string expected)
        {
            // Arrange
            IOTPClaim otpClaim = new OTPClaim(username, otp, new DateTime(year, month, day, hour, minute, second));

            // Act
            List<string> results = authenticationService.Authenticate(otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
