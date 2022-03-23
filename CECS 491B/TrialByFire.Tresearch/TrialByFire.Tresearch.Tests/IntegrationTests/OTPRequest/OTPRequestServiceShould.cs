using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.OTPRequest
{
    public class OTPRequestServiceShould : TestBaseClass
    {
        public OTPRequestServiceShould() : base(new string[] { })
        {
            TestBuilder.Services.AddScoped<IOTPRequestService, OTPRequestService>();
            TestApp = TestBuilder.Build();
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "abcDEF123", "user", "200: Server: success")]
        [InlineData("drakat7@gmail.com", "abcDEF123", "admin", "200: Server: success")]
        [InlineData("aarry@gmail.com", "#$%", "user", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdef#$%", "user", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdEF123", "user", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcDEF123", "admin", "404: Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("darry@gmail.com", "abcDEF123", "user", "404: Database: The account was not found or it " +
            "has been disabled.")]
        [InlineData("earry@gmail.com", "abcDEF123", "user", "401: Database: Please confirm your " +
            "account before attempting to login.")]
        public async Task RequestTheOTP(string username, string passphrase, string authorizationLevel, string expected)
        {
            // Arrange
            IOTPRequestService otpRequestService = TestApp.Services.GetService<IOTPRequestService>();
            IAccount account = new Account(username, passphrase, authorizationLevel);
            IOTPClaim otpClaim = new OTPClaim(account);
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await otpRequestService.RequestOTPAsync(account, otpClaim, 
                cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
