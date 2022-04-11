using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Authorization
{
    public class InMemoryAuthorizationServiceShould : TestBaseClass
    {
        public InMemoryAuthorizationServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("aarry@gmail.com", "user",
            "AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 
            "user", "", true)]
        [InlineData("barry@gmail.com", "admin",
            "E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159", 
            "user", "", true)]
        [InlineData("carry@gmail.com", "user",
            "CB3C47AD9DCEFA2CAC50D472CAFA954F00476EA58B11E8F2CD32E46F4C3DC1C990867A78D484BB25E8FF2FB44CE9F99F356E275E7E2684FFF714BAC11B663FBF", 
            "admin", "", false)]
        [InlineData("aarry@gmail.com", "user",
            "AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 
            "", "aarry@gmail.com", true)]
        [InlineData("barry@gmail.com", "admin",
            "E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159", 
            "", "aarry@gmail.com", false)]
        public async Task VerifyThatTheUserIsAuthorized(string currentIdentity, string currentRole,
            string userHash, string requiredAuthLevel, string identity, bool expected)
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
            IAuthorizationService authorizationService = TestProvider.GetService<IAuthorizationService>();
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            bool result = await authorizationService.VerifyAuthorizedAsync(requiredAuthLevel, 
                identity,cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
