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

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.IntegrationTests
{
    public class AuthenticationManagerShould
    {

        public void AuthenticateTheUser(string _username, string _otp, DateTime now)
        {
            // Arrange
            ISqlDAO _sqlDAO = new SqlDAO();
            ILogService _logService = new SqlLogService(_sqlDAO);
            IAuthenticationManager _authenticationManager = new AuthenticationManager(_sqlDAO, _logService);
            string expected = "success";

            // Act
            List<string> results = _authenticationManager.Authenticate(_username, _otp, now);

            // Assert
            Assert.Equal(expected, results[0]);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking
        }

    }
}
