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

namespace TrialByFire.Tresearch.Tests.UnitTests.OTPRequest
{
    public class InMemoryOTPRequestManagerShould : TestBaseClass
    {
        public InMemoryOTPRequestManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IMailService, MailService>();
            TestServices.AddScoped<IOTPRequestService, OTPRequestService>();
            TestServices.AddScoped<IAccountVerificationService, AccountVerificationService>();
            TestServices.AddScoped<IOTPRequestManager, OTPRequestManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "abcDEF123", "user", "guest", "guest", "", "200: Server: StoreOTP success.")]
        [InlineData("drakat7@gmail.com", "abcDEF123", "admin", "guest", "guest", "", "200: Server: StoreOTP success.")]
        [InlineData("aarry@gmail.com", "#$%", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdef#$%", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdEF123", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcDEF123", "admin", "guest", "guest", "", "500: Database: The UserAccount was not found.")]
        [InlineData("barry@gmail.com", "abcDEF123", "admin", "barry@yahoo.com",
                    "E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159",
                    "admin", "403: Server: Active session found. Please logout and try again.")]
        [InlineData("darry@gmail.com", "abcDEF123", "user", "guest", "guest", "", "401: Database: UserAccount disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("earry@gmail.com", "abcDEF123", "user", "guest", "guest", "", "401: Database: Please confirm your " +
            "account before attempting to login.")]
        public async Task RequestTheOTPAsync(string username, string passphrase, string authorizationLevel, 
            string currentIdentity, string currentRole, string userHash, string expected)
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
            byte[] key = KeyDerivation.Pbkdf2(passphrase, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
            string hash = Convert.ToHexString(key);
            IOTPRequestManager otpRequestManager = TestProvider.GetService<IOTPRequestManager>();

            // Act
            string result = await otpRequestManager.RequestOTPAsync(username, hash, 
                authorizationLevel).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("drakat7@gmail.com", "abcDEF123", "user", "guest", "guest", "", "200: Server: StoreOTP success.")]
        [InlineData("drakat7@gmail.com", "abcDEF123", "admin", "guest", "guest", "", "200: Server: StoreOTP success.")]
        [InlineData("aarry@gmail.com", "#$%", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdef#$%", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcdEF123", "user", "guest", "guest", "", "400: Data: Invalid Username or " +
            "Passphrase. Please try again.")]
        [InlineData("aarry@gmail.com", "abcDEF123", "admin", "guest", "guest", "", "500: Database: The UserAccount was not found.")]
        [InlineData("barry@gmail.com", "abcDEF123", "admin", "barry@yahoo.com", 
                    "E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159", 
                    "admin", "403: Server: Active session found. Please logout and try again.")]
        [InlineData("darry@gmail.com", "abcDEF123", "user", "guest", "guest", "", "401: Database: UserAccount disabled. " +
            "Perform account recovery or contact system admin.")]
        [InlineData("earry@gmail.com", "abcDEF123", "user", "guest", "guest", "", "401: Database: Please confirm your " +
            "account before attempting to login.")]
        public async Task RequestTheOTPAsyncWithin5Seconds(string username, string passphrase, 
            string authorizationLevel, string currentIdentity, string currentRole, string userHash, 
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
            byte[] salt = new byte[0];
            byte[] key = KeyDerivation.Pbkdf2(passphrase, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
            string hash = Convert.ToHexString(key);
            IOTPRequestManager otpRequestManager = TestProvider.GetService<IOTPRequestManager>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            string result = await otpRequestManager.RequestOTPAsync(username, hash,
                authorizationLevel, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
