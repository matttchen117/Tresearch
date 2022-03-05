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

namespace TrialByFire.Tresearch.Tests.UnitTests.OTPRequest
{
    public class InMemoryOTPRequestManagerShould : InMemoryTestDependencies
    {
        public InMemoryOTPRequestManagerShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "abcDEF123", "guest", "guest", "success")]
        [InlineData("larry@gmail.com", "#$%", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdef#$%", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("larry@gmail.com", "abcdEF123", "guest", "guest", "Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("billy@yahoo.com", "abcDEF123", "billy@yahoo.com", "admin", "Server: Active session found. " +
            "Please logout and try again.")]
        [InlineData("joe@outlook.com", "abcDEF123", "guest", "guest", "success")]
        [InlineData("bob@yahoo.com", "abcDEF123", "guest", "guest", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("harry@yahoo.com", "abcDEF123", "guest", "guest", "Database: Please confirm your " +
            "account before attempting to login.")]
        public void RequestTheOTP(string username, string passphrase, string currentIdentity, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService,
                authenticationService, rolePrincipal, otpRequestService, messageBank);

            // Act
            string result = otpRequestManager.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
