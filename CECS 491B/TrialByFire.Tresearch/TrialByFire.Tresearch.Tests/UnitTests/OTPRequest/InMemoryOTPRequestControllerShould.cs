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
        [InlineData("larry@gmail.com", "abcDEF123", "guest", "guest", "success")]
        [InlineData("billy@yahoo.com", "abcDEF123", "billy@yahoo.com", "admin", "Server: User is already authenticated.")]
        [InlineData("joe@outlook.com", "abcDEF123", "guest", "guest", "success")]
        [InlineData("bob@yahoo.com", "abcDEF123", "guest", "guest", "Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("harry@yahoo.com", "abcDEF123", "guest", "guest", "Database: Please click on the confirmation link that " +
            "we sent to your email in order to confirm your account.")]
        public void RequestTheOTP(string username, string passphrase, string currentIdentity, string currentRole, 
            string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService, 
                authenticationService, rolePrincipal, otpRequestService);
            IOTPRequestController otpRequestController = new OTPRequestController(sqlDAO, logService, otpRequestManager);

            // Act
            string result = otpRequestController.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test()
        {
            Assert.True(true);
        }
    }
}
