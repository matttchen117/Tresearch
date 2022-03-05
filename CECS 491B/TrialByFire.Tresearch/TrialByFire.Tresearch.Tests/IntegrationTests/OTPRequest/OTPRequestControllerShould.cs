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

namespace TrialByFire.Tresearch.Tests.OTPRequestTests
{
    public class OTPRequestControllerShould
    {
        public void RequestTheOTP(string username, string passphrase)
        {
            // Arrange
            ISqlDAO sqlDAO = new InMemorySqlDAO();
            ILogService logService = new InMemoryLogService(sqlDAO);
            IValidationService validationService = new ValidationService();
            IAuthenticationService authenticationService = new AuthenticationService(sqlDAO, logService);
            IRoleIdentity roleIdentity = new RoleIdentity(true, "Bob", "User");
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, validationService,
                authenticationService, rolePrincipal, otpRequestService);
            IOTPRequestController otpRequestController = new OTPRequestController(sqlDAO, logService, otpRequestManager);
            string expected = "success";

            // Act
            string result = otpRequestController.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
