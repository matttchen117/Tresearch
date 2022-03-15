using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Authentication
{
    public class AuthenticationControllerShould : IntegrationTestDependencies
    {
        public AuthenticationControllerShould() : base()
        {
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "503: Server: Authentication Cookie creation failed.")]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "garry@gmail.com", "user", 2022, 3, 4, 5, 6, 0, "403: Server: Active session found. Please logout and try again.")]
        [InlineData("harry@gmail.com", "abcdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("harry@gmail.com", "abc", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("harry@gmail.com", "abcdef#$%", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("harrygmail.com", "ABCdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "404: Database: The account was not found " +
            "or it has been disabled.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", "guest", "guest", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "404: Database: The account was not found " +
            "or it has been disabled.")]
        [InlineData("marry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "401: Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("narry@gmail.com", "abcdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUser(string username, string otp, string authorizationLevel, string currentIdentity, string currentRole,
            int year, int month, int day, int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IAuthenticationManager authenticationManager = new AuthenticationManager(sqlDAO,
                logService, validationService, authenticationService, rolePrincipal, messageBank);
            IAuthenticationController authenticationController = new AuthenticationController(sqlDAO,
                logService, authenticationManager, messageBank);
            DateTime now = new DateTime(year, month, day, hour, minute, second);
            string[] expecteds = expected.Split(": ");
            ObjectResult expectedResult = new ObjectResult(expecteds[2])
            { StatusCode = Convert.ToInt32(expecteds[0]) };

            // Act
            IActionResult result = await authenticationController.AuthenticateAsync(username, otp, authorizationLevel, now);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
