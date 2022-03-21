using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.OTPRequest
{
    public class OTPRequestControllerShould : IntegrationTestDependencies
    {
        public OTPRequestControllerShould() : base()
        {
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "abcDEF123", "user", "guest", "guest", "503: Server: Email failed to send.")]
        [InlineData("drakat7@gmail.com", "abcDEF123", "admin", "guest", "guest", "503: Server: Email failed to send.")]
        [InlineData("aarry@gmail.com", "#$%", "user", "guest", "guest", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdef#$%", "user", "guest", "guest", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdEF123", "user", "guest", "guest", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcDEF123", "admin", "guest", "guest", "404: Database: The account was not found or " +
            "it has been disabled.")]
        [InlineData("barry@gmail.com", "abcDEF123", "admin", "billy@yahoo.com", "admin", "403: Server: Active session found. " +
            "Please logout and try again.")]
        [InlineData("darry@gmail.com", "abcDEF123", "user", "guest", "guest", "404: Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("earry@gmail.com", "abcDEF123", "user", "guest", "guest", "401: Database: Please confirm your " +
            "account before attempting to login.")]
        public async Task RequestTheOTP(string username, string passphrase, string authorizationLevel, string currentIdentity, string currentRole,
            string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            IMailService mailService = new MailService(MessageBank);
            IOTPRequestService otpRequestService = new OTPRequestService(SqlDAO, LogService, MessageBank);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(SqlDAO, LogService, ValidationService,
                AuthenticationService, otpRequestService, MessageBank, mailService);
            IOTPRequestController otpRequestController = new OTPRequestController(SqlDAO, LogService,
                otpRequestManager, MessageBank);
            string[] expecteds = expected.Split(": ");
            ObjectResult expectedResult = new ObjectResult(expecteds[2])
            { StatusCode = Convert.ToInt32(expecteds[0]) };

            // Act
            IActionResult result = await otpRequestController.RequestOTPAsync(username, passphrase, 
                authorizationLevel).ConfigureAwait(false);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.Equal(expectedResult.StatusCode, objectResult.StatusCode);
            Assert.Equal(expectedResult.Value, objectResult.Value);
        }
    }
}
