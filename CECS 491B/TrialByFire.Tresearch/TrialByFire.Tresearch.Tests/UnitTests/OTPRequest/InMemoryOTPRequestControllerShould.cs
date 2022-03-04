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
    public class InMemoryOTPRequestControllerShould
    {
        public void RequestTheOTP(string username, string passphrase)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IValidationService validationService = new ValidationService();
            IAuthenticationService authenticationService = new AuthenticationService(inMemorySqlDAO, inMemoryLogService);
            IRoleIdentity roleIdentity = new RoleIdentity(true, "Bob", "User");
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IOTPRequestService otpRequestService = new OTPRequestService(inMemorySqlDAO, inMemoryLogService);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(inMemorySqlDAO, inMemoryLogService, validationService, 
                authenticationService, rolePrincipal, otpRequestService);
            IOTPRequestController otpRequestController = new OTPRequestController(inMemorySqlDAO, inMemoryLogService, otpRequestManager);
            string expected = "success";

            // Act
            string result = otpRequestController.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
