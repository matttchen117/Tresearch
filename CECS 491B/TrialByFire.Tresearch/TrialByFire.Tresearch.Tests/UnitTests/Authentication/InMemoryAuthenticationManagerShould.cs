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
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationManagerShould
    {

        public void AuthenticateTheUser(string _username, string _otp, DateTime now)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IAuthenticationService authenticationService = new AuthenticationService(inMemorySqlDAO, authenticationService);
            IAuthenticationManager authenticationManager = new AuthenticationManager(inMemorySqlDAO, inMemoryLogService, authenticationService);
            string expected = "success";

            // Act
            List<string> results = authenticationManager.Authenticate(_username, _otp, now);

            // Assert
            Assert.Equal(expected, results[0]);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking
        }

    }
}
