using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.AuthenticationTests.UnitTests
{
    public class InMemoryAuthenticationManagerShould
    {

        public void AuthenticateTheUser(string _username, string _otp, DateTime now)
        {
            // Arrange
            IAuthenticationManager _inMemoryAuthenticationManager = new InMemoryAuthenticationManager();
            string expected = "success";

            // Act
            List<string> results = _inMemoryAuthenticationManager.Authenticate(_username, _otp, now);

            // Assert
            Assert.Equal(expected, results[0]);

            // Not unit test if connecting to outside db
            // Unit test if using in memory/turn into unit with mocking
        }

    }
}
