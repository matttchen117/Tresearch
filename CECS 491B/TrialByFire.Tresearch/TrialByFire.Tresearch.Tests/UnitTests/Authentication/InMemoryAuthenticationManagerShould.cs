using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        [InlineData("garry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "garry@gmail.com", "user",
                    "8C89E7886643911D171624EEFDF875F6B45C0052A761A6A713E9D26EFCF66F9D47ADD4899E98C6A8525CCB7D68F9BAB1EF1A75D4F1558726103FF0BE7B6A32B2",
                    2022, 3, 4, 5, 6, 0, "403: Server: Active session found. Please logout and try again.")]
        [InlineData("iarry@gmail.com", "abcdef123", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abc", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abcdef#$%", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarrygmail.com", "ABCdef123", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "500: Database: The UserAccount was not found.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "401: Database: UserAccount disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("marry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "401: Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("oarry@gmail.com", "abcdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsync(string username, string otp, string authorizationLevel, 
            string currentIdentity, string currentRole, string userHash, int year, int month, int day, 
            int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");

            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            byte[] salt = new byte[0];
            byte[] key = KeyDerivation.Pbkdf2(otp, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
            string hash = Convert.ToHexString(key);
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            DateTime now = new DateTime(year, month, day, hour, minute, second);

            // Act
            List<string> results = await authenticationManager.AuthenticateAsync(username, hash,
                authorizationLevel, now).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        [Theory]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "200: Server: Authentication success.")]
        [InlineData("garry@gmail.com", "ABCdef123", "user", "garry@gmail.com", "user", 
                    "8C89E7886643911D171624EEFDF875F6B45C0052A761A6A713E9D26EFCF66F9D47ADD4899E98C6A8525CCB7D68F9BAB1EF1A75D4F1558726103FF0BE7B6A32B2", 
                    2022, 3, 4, 5, 6, 0, "403: Server: Active session found. Please logout and try again.")]
        [InlineData("iarry@gmail.com", "abcdef123", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abc", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarry@gmail.com", "abcdef#$%", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Data: Invalid Username or OTP. " +
            "Please try again.")]
        [InlineData("iarrygmail.com", "ABCdef123", "admin", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "500: Database: The UserAccount was not found.")]
        [InlineData("karry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2023, 3, 4, 5, 6, 0, "400: Data: The OTP has expired. Please request " +
            "a new one.")]
        [InlineData("larry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "401: Database: UserAccount disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("marry@gmail.com", "ABCdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "401: Database: Please confirm your " +
            "account before attempting to login.")]
        [InlineData("oarry@gmail.com", "abcdef123", "user", "guest", "guest", "", 2022, 3, 4, 5, 6, 0, "400: Database: Too many fails have occurred. " +
            "The account has been disabled.")]
        public async Task AuthenticateTheUserAsyncWithin5Seconds(string username, string otp, 
            string authorizationLevel, string currentIdentity, string currentRole, string userHash,
            int year, int month, int day, int hour, int minute, int second, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");

            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            byte[] salt = new byte[0];
            byte[] key = KeyDerivation.Pbkdf2(otp, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
            string hash = Convert.ToHexString(key);
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            DateTime now = new DateTime(year, month, day, hour, minute, second);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));

            // Act
            List<string> results = await authenticationManager.AuthenticateAsync(username, hash, 
                authorizationLevel, now, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, results[0]);
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "user", 
                    "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    "refreshSessionSuccess")]
        [InlineData("guest", "guest", "", "refreshSessionNotAllowed")]
        public async Task RefreshTheSessionAsync(string currentIdentity, string currentRole, string userHash, 
            string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");

            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            Enum.TryParse(expected, out IMessageBank.Responses response);
            string expectedResult = await messageBank.GetMessage(response).ConfigureAwait(false);

            // Act
            List<string> results = await authenticationManager.RefreshSessionAsync().ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResult, results[0]);
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "user",
                    "0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6",
                    "refreshSessionSuccess")]
        [InlineData("guest", "guest", "", "refreshSessionNotAllowed")]
        public async Task RefreshTheSessionAsyncWithin5Seconds(string currentIdentity, 
            string currentRole, string userHash, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity;
            IRolePrincipal rolePrincipal;
            if (!currentIdentity.Equals("guest"))
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userHash);
            }
            else
            {
                roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, "E64C56A055B741393268F7EE26EF3AA00FC58D6272C06DE31932B71EB965A68CCB9F32AEBD74E25708AD501C7D7AAA1E5C4CE4C9010149FBA08B2C5351A57F34");

            }
            rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IAuthenticationManager authenticationManager = TestProvider.GetService<IAuthenticationManager>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            Enum.TryParse(expected, out IMessageBank.Responses response);
            string expectedResult = await messageBank.GetMessage(response).ConfigureAwait(false);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));

            // Act
            List<string> results = await authenticationManager.RefreshSessionAsync(cancellationTokenSource.Token)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResult, results[0]);
        }
    }
}
