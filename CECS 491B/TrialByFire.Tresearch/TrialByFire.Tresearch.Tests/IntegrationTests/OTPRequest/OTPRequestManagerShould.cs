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

namespace TrialByFire.Tresearch.Tests.OTPRequestTests
{
    public class OTPRequestManagerShould
    {
        public void RequestTheOTP(string username, string passphrase)
        {
            // Arrange
            ISqlDAO sqlDAO = new SqlDAO();
            ILogService logService = new SqlLogService(sqlDAO);
            IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
            IOTPRequestManager otpRequestManager = new OTPRequestManager(sqlDAO, logService, otpRequestService);
            string expected = "success";

            // Act
            string result = otpRequestManager.RequestOTP(username, passphrase);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
