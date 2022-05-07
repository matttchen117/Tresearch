using Microsoft.AspNetCore.Mvc;
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

namespace TrialByFire.Tresearch.Tests.UnitTests.PrivateAndPublic
{
    public class InMemoryPrivateAndPublicServiceShould : TestBaseClass
    {

        private IPrivateAndPublicService _privateAndPublicService;

        public InMemoryPrivateAndPublicServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IPrivateAndPublicService, PrivateAndPublicService>();
            TestProvider = TestServices.BuildServiceProvider();
            _privateAndPublicService = TestProvider.GetService<IPrivateAndPublicService>();
        }


        [Theory]
        [MemberData(nameof(PrivateNodeData))]

        public async Task PrivateNodeAsync(IRoleIdentity roleIdentity, List<long> nodes, IResponse<string> expected)
        {

            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            // Act
            IResponse<string> response = await _privateAndPublicService.PrivateNodeAsync(nodes).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Data);

        }


        public static IEnumerable<object[]> PrivateNodeData()
        {



            //user is guest 
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "guest", "guest", "");
            var nodesToPrivate0 = new List<long>();
            IResponse<string> nodesPrivateResult0 = new PrivateResponse<string>("401: Server: No active session found. Please login and try again.", null, 400, false);


            //user is not guest
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "grizzly@gmail.com", "user", "87ec69f0ab41c3dcb31e01dcf9942d756501b421887524a1e691dff69a698cf1d46c26b68f73dddb29a7d2729eddf43580bab9a5002d2289c0c7bf4d5db7c7ae");
            var nodesToPrivate1 = new List<long> { 1, 2, 3 };
            IResponse<string> nodesPrivateResult1 = new PrivateResponse<string>("200: Server: Private Node Success", null, 200, true);


            return new[]
            {

                new object[] { roleIdentity0, nodesToPrivate0, nodesPrivateResult0},
                new object[] { roleIdentity1, nodesToPrivate1, nodesPrivateResult1},

            };

        }


        [Theory]
        [MemberData(nameof(PublicNodeData))]

        public async Task PublicNodeAsync(IRoleIdentity roleIdentity, List<long> nodes, IResponse<string> expected)
        {

            // Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            // Act
            IResponse<string> response = await _privateAndPublicService.PublicNodeAsync(nodes).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Data);

        }

        //don't actually need to check for role here
        //since private node can only be seen by owner

        public static IEnumerable<object[]> PublicNodeData()
        {
            //user is guest 
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "guest", "guest", "");
            var nodesToPrivate0 = new List<long>();
            IResponse<string> nodesPrivateResult0 = new PublicResponse<string>("401: Server: No active session found. Please login and try again.", null, 400, false);


            //user is not guest
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "grizzly@gmail.com", "user", "87ec69f0ab41c3dcb31e01dcf9942d756501b421887524a1e691dff69a698cf1d46c26b68f73dddb29a7d2729eddf43580bab9a5002d2289c0c7bf4d5db7c7ae");
            var nodesToPrivate1 = new List<long> { 1, 2, 3 };
            IResponse<string> nodesPrivateResult1 = new PublicResponse<string>("200: Server: Public Node Success", null, 200, true);


            return new[]
            {

                new object[] { roleIdentity0, nodesToPrivate0, nodesPrivateResult0},
                new object[] { roleIdentity1, nodesToPrivate1, nodesPrivateResult1},

            };
        }




    }
}
