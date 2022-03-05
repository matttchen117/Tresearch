using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
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
        public void AuthenticateTheUser(string username, string otp)
        {
            /*// Arrange
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(sqlDAO, logService);
            IAuthenticationManager authenticationManager = new AuthenticationManager(sqlDAO, logService, authenticationService);
            IAuthenticationController authenticationController = new AuthenticationController(sqlDAO, logService, authenticationManager);
            string expected = "success";

            // Act
            List<string> results = authenticationController.Authenticate(username, otp, DateTime.Now);

            // Assert
            Assert.Equal(expected, results[0]);*/
        }

    }
}
