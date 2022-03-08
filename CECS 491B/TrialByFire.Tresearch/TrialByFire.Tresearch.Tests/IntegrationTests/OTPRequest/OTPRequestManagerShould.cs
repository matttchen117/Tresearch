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
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.OTPRequest
{
    public class OTPRequestManagerShould : IntegrationTestDependencies
    {
        public OTPRequestManagerShould() : base()
        {
        }
        [Theory]
        [InlineData("drakat7@gmail.com", "abcDEF123", "user", "guest", "guest", "success")]
        [InlineData("drakat7@gmail.com", "abcDEF123", "admin", "guest", "guest", "success")]
        [InlineData("aarry@gmail.com", "#$%", "guest", "user", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdef#$%", "user", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdEF123", "user", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcDEF123", "admin", "guest", "guest", "Database: The account was not found or " +
            "it has been disabled.")]
        [InlineData("barry@gmail.com", "abcDEF123", "admin", "billy@yahoo.com", "admin", "Server: Active session found. " +
            "Please logout and try again.")]
        [InlineData("darry@gmail.com", "abcDEF123", "user", "guest", "guest", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("earry@gmail.com", "abcDEF123", "user", "guest", "guest", "Database: Please confirm your " +
            "account before attempting to login.")]
        public void RequestTheOTP(string username, string passphrase, string authorizationLevel, string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IMailService mailService = new MailService(messageBank);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService, messageBank);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService,
                authenticationService, rolePrincipal, otpRequestService, messageBank, mailService);

            // Act
            string result = otpRequestManager.RequestOTP(username, passphrase, authorizationLevel);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
