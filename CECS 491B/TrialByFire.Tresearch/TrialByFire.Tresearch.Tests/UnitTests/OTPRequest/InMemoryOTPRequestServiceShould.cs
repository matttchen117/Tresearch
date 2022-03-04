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
    public class InMemoryOTPRequestServiceShould
    {
        public void RequestTheOTP(string username, string passphrase)
        {
            // Arrange
            ISqlDAO inMemorySqlDAO = new InMemorySqlDAO();
            ILogService inMemoryLogService = new InMemoryLogService(inMemorySqlDAO);
            IOTPRequestService otpRequestService = new OTPRequestService(inMemorySqlDAO, inMemoryLogService);
            IAccount account = new Account(username, passphrase);
            string expected = "success";

            // Act
            string result = otpRequestService.RequestOTP(account);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
