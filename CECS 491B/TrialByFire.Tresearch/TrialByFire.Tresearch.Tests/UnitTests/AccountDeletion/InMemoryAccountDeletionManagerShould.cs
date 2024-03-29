﻿using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.AccountDeletion
{
    public class InMemoryAccountDeletionManagerShould : TestBaseClass
    {
        public InMemoryAccountDeletionManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IAccountDeletionService, AccountDeletionService>();
            TestServices.AddScoped<IAccountDeletionManager, AccountDeletionManager>();
            TestProvider = TestServices.BuildServiceProvider();

        }

        [Theory]
        [InlineData("trizip@gmail.com", "user", "200: Server: Account Deletion Successful.")]
        [InlineData("switchblade@gmail.com", "admin", "200: Server: Account Deletion Successful.")]
        [InlineData("greenKeyCard@gmail.com", "user", "500: Database: The Account was not found.")]

        public async Task DeleteTheUserAsync(string currentIdentity, string currentRole, string expected)
        {

            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, currentIdentity, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!currentIdentity.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }


            IAccountDeletionManager accountDeletionManager = TestProvider.GetService<IAccountDeletionManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5));


            // Act
            string result = await accountDeletionManager.DeleteAccountAsync(cancellationTokenSource.Token).ConfigureAwait(false);


            // Assert
            Assert.Equal(expected, result);



        }


    }
}