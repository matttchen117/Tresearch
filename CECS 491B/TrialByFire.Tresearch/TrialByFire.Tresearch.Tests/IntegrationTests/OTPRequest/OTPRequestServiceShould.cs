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

namespace TrialByFire.Tresearch.Tests.OTPRequestTests
{
    public class OTPRequestServiceShould
    {
        [Theory]
        [InlineData("larry@gmail.com", "abcDEF123")]
        [InlineData("billy@yahoo.com", "abcDEF123")]
        public void RequestTheOTP(string username, string passphrase)
        {
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IAccount account = new Account(username, passphrase);
            IOTPClaim otpClaim = new OTPClaim(account);
            string expected = "success";

            // Act
            string result = otpRequestService.RequestOTP(account, otpClaim);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
