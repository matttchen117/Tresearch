using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authentication
{
    public class InMemoryAuthenticationManagerShould : InMemoryTestDependencies
    {
        public InMemoryAuthenticationManagerShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "success")]
        [InlineData("billy@yahoo.com","abcdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "abc", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com","abcdef#$%", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "abcdefghi", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billyyahoocom", "ABCdef123", "guest", "admin", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("joe@outlook.com", "ABCdef123", "user", "guest", "guest", 2023, 3, 4, 5, 6, 0, "Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("bob@yahoo.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Database: The account was not found " +
            "or it has been disabled.")]
        [InlineData("harry@yahoo.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("barry@yahoo.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 10, 0, "Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public void AuthenticateTheUser(string username, string otp, string authorizationLevel, string currentIdentity, string currentRole,
            int year, int month, int day, int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IAuthenticationManager authenticationManager = new AuthenticationManager(sqlDAO,
                logService, validationService, authenticationService, rolePrincipal, messageBank);
            DateTime now = new DateTime(year, month, day, hour, minute, second);

            // Act
            List<string> results = authenticationManager.Authenticate(username, otp, authorizationLevel, now);

            // Assert
            Assert.Equal(expected, results[0]);
        }

    }
}
