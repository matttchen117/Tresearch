using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authentication
{
    public class InMemoryAuthenticationManagerShould : TestBaseClass
    {
        public InMemoryAuthenticationManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IAccountVerificationService, AccountVerificationService>();
            TestServices.AddScoped<IAuthenticationManager, AuthenticationManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "garry@gmail.com", "user", 2022, 3, 4, 5, 6, 0, "403: Server: Active session found. Please logout and try again.")]
        [InlineData("iarry@gmail.com", "abcdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abc", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abcdef#$%", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarrygmail.com", "ABCdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "500: Database: The Account was not found.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", "guest", "guest", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "401: Database: Account disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("marry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "401: Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("oarry@gmail.com", "abcdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsync(string username, string otp, string authorizationLevel, string currentIdentity, string currentRole,
            int year, int month, int day, int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = new ClaimsPrincipal(rolePrincipal);
            }
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            DateTime now = new DateTime(year, month, day, hour, minute, second);

            // Act
            List<string> results = await authenticationManager.AuthenticateAsync(username, otp,
                authorizationLevel, now).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "garry@gmail.com", "user", 2022, 3, 4, 5, 6, 0, "403: Server: Active session found. Please logout and try again.")]
        [InlineData("iarry@gmail.com", "abcdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abc", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abcdef#$%", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarrygmail.com", "ABCdef123", "admin", "guest", "guest", 2022, 3, 4, 5, 6, 0, "500: Database: The Account was not found.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", "guest", "guest", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "401: Database: Account disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("marry@gmail.com", "ABCdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "401: Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("oarry@gmail.com", "abcdef123", "user", "guest", "guest", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsyncWithin5Seconds(string username, string otp, string authorizationLevel, string currentIdentity, string currentRole,
            int year, int month, int day, int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if(!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = new ClaimsPrincipal(rolePrincipal);
            }
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            DateTime now = new DateTime(year, month, day, hour, minute, second);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));

            // Act
            List<string> results = await authenticationManager.AuthenticateAsync(username, otp, 
                authorizationLevel, now, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);
        }

    }
}
