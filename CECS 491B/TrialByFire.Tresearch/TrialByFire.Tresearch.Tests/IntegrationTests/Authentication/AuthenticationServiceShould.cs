using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Authentication
{
    public class AuthenticationServiceShould : TestBaseClass
    {
        public AuthenticationServiceShould() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "admin", 2022, 3, 4, 5, 6, 0, "500: Database: The OTP Claim was not found.")]
        [InlineData("jarry@gmail.com", "abcdef123", "admin", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("jarry@gmail.com", "abcdefghi", "admin", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("parry@gmail.com", "abcdef123", "user", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsync(string username, string otp, string authorizationLevel, int year, int month, int day, int hour,
            int minute, int second, string expected)
        {
            // Arrange
            IOTPClaim otpClaim = new OTPClaim(username, otp, authorizationLevel, new DateTime(year, month, day, hour, minute, second));
            IAuthenticationService authenticationService = TestProvider.GetService<IAuthenticationService>();

            // Act
            List<string> results = await authenticationService.AuthenticateAsync(otpClaim)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);

        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "admin", 2022, 3, 4, 5, 6, 0, "500: Database: The OTP Claim was not found.")]
        [InlineData("jarry@gmail.com", "abcdef123", "admin", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("jarry@gmail.com", "abcdefghi", "admin", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("parry@gmail.com", "abcdef123", "user", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsyncWithin5Seconds(string username, string otp, string authorizationLevel, int year, int month, int day, int hour,
            int minute, int second, string expected)
        {
            // Arrange
            IOTPClaim otpClaim = new OTPClaim(username, otp, authorizationLevel, new DateTime(year, month, day, hour, minute, second));
            IAuthenticationService authenticationService = TestProvider.GetService<IAuthenticationService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            List<string> results = await authenticationService.AuthenticateAsync(otpClaim, 
                cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);

        }
    }
}
