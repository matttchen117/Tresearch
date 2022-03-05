using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.OTPRequestTests
{
    public class OTPRequestServiceShould
    {
        // Arrange
        ISqlDAO sqlDAO = new SqlDAO();
        ILogService logService = new SqlLogService(sqlDAO);
        IOTPRequestService otpRequestService = new OTPRequestService(sqlDAO, logService);
        string expected = "success";

        // Act
        string result = otpRequestService.RequestOTP(username, passphrase);

        // Assert
        Assert.Equal(expected, result);
    }
}
