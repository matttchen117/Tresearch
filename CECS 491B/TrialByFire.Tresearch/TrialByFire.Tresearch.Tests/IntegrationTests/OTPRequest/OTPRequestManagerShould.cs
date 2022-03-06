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
        public void RequestTheOTP(string username, string passphrase, string role)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, "Bob", "User");
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService, messageBank);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService,
                authenticationService, rolePrincipal, otpRequestService, messageBank);
            string expected = "success";

            // Act
            string result = otpRequestManager.RequestOTP(username, passphrase, role);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
