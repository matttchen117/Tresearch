using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.OTPRequest
{
    public class OTPRequestServiceShould : IntegrationTestDependencies
    {
        public OTPRequestServiceShould() : base()
        {

        }

        public void RequestTheOTP(string username, string passphrase)
        {
            /*// Arrange
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            string expected = "success";

            // Act
            string result = otpRequestService.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);*/

        }
    }
}
