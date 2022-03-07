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
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authentication
{
    public class InMemoryAuthenticationServiceShould : InMemoryTestDependencies
    {
        public InMemoryAuthenticationServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", 2022, 3, 4, 5, 6, 0, "success")]
        [InlineData("garry@gmail.com", "ABCdef123", "admin", 2022, 3, 4, 5, 6, 0, "Database: The account was not found " +
            "or it has been disabled.")]
        [InlineData("harry@gmail.com", "abcdef123", "admin", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("harry@gmail.com", "abcdefghi", "admin", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "ABCdef123", "user", 2023, 3, 4, 5, 6, 0, "Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("jarry@gmail.com", "ABCdef123", "user", 2022, 3, 4, 5, 6, 0, "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", 2022, 3, 4, 5, 6, 0, "Database: Please confirm your account " +
            "before attempting to login.")]
        [InlineData("larry@gmail.com", "abcdef123", "user", 2022, 3, 4, 5, 6, 0, "Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public void AuthenticateTheUser(string username, string otp, string authorizationLevel, int year, int month, int day, int hour,
            int minute, int second, string expected)
        {
            // Arrange
            IOTPClaim otpClaim = new OTPClaim(username, otp, authorizationLevel, new DateTime(year, month, day, hour, minute, second));

            // Act
            List<string> results = authenticationService.Authenticate(otpClaim);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
