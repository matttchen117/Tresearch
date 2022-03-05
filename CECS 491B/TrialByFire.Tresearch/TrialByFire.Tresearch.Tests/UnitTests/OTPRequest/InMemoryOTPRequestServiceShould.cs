using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.OTPRequest
{
    public class InMemoryOTPRequestServiceShould : InMemoryTestDependencies
    {
        public InMemoryOTPRequestServiceShould() : base()
        {
        }

        [Theory]
        [InlineData("larry@gmail.com", "abcDEF123", "success")]
        [InlineData("billy@yahoo.com", "abcDEF123", "success")]
        [InlineData("joe@outlook.com", "abcDEF123", "success")]
        [InlineData("bob@yahoo.com", "abcDEF123", "Database: The account was not found or it has been disabled.")]
        [InlineData("harry@yahoo.com", "abcDEF123", "Database: Please click on the confirmation link that " +
            "we sent to your email in order to confirm your account.")]
        public void RequestTheOTP(string username, string passphrase, string expected)
        {
            // Arrange
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IAccount account = new Account(username, passphrase);
            IOTPClaim otpClaim = new OTPClaim(account);

            // Act
            string result = otpRequestService.RequestOTP(account, otpClaim);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
