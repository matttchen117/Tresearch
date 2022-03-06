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
using TrialByFire.Tresearch.WebApi.Controllers;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authentication
{
    public class InMemoryAuthenticationControllerShould : InMemoryTestDependencies
    {
        public InMemoryAuthenticationControllerShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "user", "ABCdef123", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Server: Authentication Cookie creation failed")]
        [InlineData("billy@yahoo.com", "admin", "abcdef123", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "admin", "abc", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "admin", "abcdef#$%", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billy@yahoo.com", "admin", "abcdefghi", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("billyyahoocom", "admin", "ABCdef123", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("joe@outlook.com", "user", "ABCdef123", "guest", "guest", 2023, 3, 4, 5, 6, 0, "Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("bob@yahoo.com", "user", "ABCdef123", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Database: The account was not found " +
            "or it has been disabled.")]
        [InlineData("harry@yahoo.com", "user", "ABCdef123", "guest", "guest", 2022, 3, 4, 5, 6, 0, "Database: Please confirm your " +
            "account before attempting to login.")]
        public void AuthenticateTheUser(string username, string role, string otp, string currentIdentity, string currentRole,
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

            // Act
            string result = authenticationController.Authenticate(username, otp, now);

            // Assert
            Assert.Equal(expected, result);
        }

        /*        public void CreateTheCookie(string username, string otp)
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
                }*/

    }
}
