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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.OTPRequest
{
    public class InMemoryOTPRequestControllerShould : InMemoryTestDependencies
    {
        public InMemoryOTPRequestControllerShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "abcDEF123", "user", "guest", "guest", "success")]
        [InlineData("larry@gmail.com", "#$%", "guest", "user", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdef#$%", "user", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdEF123", "user", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcDEF123", "admin", "guest", "guest", "Database: The account was not found or " +
            "it has been disabled.")]
        [InlineData("billy@yahoo.com", "abcDEF123", "admin", "billy@yahoo.com", "admin", "Server: Active session found. " +
            "Please logout and try again.")]
        [InlineData("joe@outlook.com", "abcDEF123", "user", "guest", "guest", "success")]
        [InlineData("bob@yahoo.com", "abcDEF123", "user", "guest", "guest", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("harry@yahoo.com", "abcDEF123", "user", "guest", "guest", "Database: Please confirm your " +
            "account before attempting to login.")]
        public void RequestTheOTP(string username, string passphrase, string role, string currentIdentity, string currentRole, 
            string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IMailService mailService = new MailService(messageBank);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService, messageBank);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService, 
                authenticationService, rolePrincipal, otpRequestService, messageBank, mailService);
            IOTPRequestController otpRequestController = new OTPRequestController(sqlDAO, logService, 
                otpRequestManager);

            // Act
            string result = otpRequestController.RequestOTP(username, passphrase, role);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
