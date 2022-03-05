using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Authentication
{
    public class AuthenticationManagerShould
    {

        public void AuthenticateTheUser(string username, string otp, DateTime now)
        {
            /*// Arrange
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(sqlDAO, logService);
            IAuthenticationManager authenticationManager = new AuthenticationManager(sqlDAO, logService, authenticationService);
            string expected = "success";

            // Act
            List<string> results = authenticationManager.Authenticate(username, otp, now);

            // Assert
            Assert.Equal(expected, results[0]);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking*/
        }

    }
}
