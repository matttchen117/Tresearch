using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TrialByFire.Tresearch.Tests.IntegrationTests.PrivateAndPublic
{
    public class PrivateAndPublicServiceShould : TestBaseClass
    {
        private IPrivateAndPublicService _privateAndPublicService;

        public PrivateAndPublicServiceShould() : base(new string[] { })
        {
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
            IResponse<string> response = await _privateAndPublicService.PrivateNodeAsync(nodes);

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
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "viet.nguyen03@student.csulb.edu", "user", "75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6");
            var nodesToPrivate1 = new List<long> { 24, 25 };
            IResponse<string> nodesPrivateResult1 = new PrivateResponse<string>("", "200: Server: Private Node Success", 200, true);


            /*
            //nodes are not theirs to private/public
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "altyn@gmail.com", "user", "daa1f5b0a47533d633352b244a99a67d2f10566f29c9a42145e99efbf12a9859fca0830cd7f44abd3c1ceee03f6ac03b9d36c33394b8681359577002a2d3ddce");
            var nodesToPrivate2 = new List<long> { 24, 25 };
            IResponse<string> nodesPrivateResult2 = new PrivateResponse<string>("403: Database: You are not authorized to perform this operation.", null, 403, false);
            */


            return new[]
            {

                new object[] { roleIdentity0, nodesToPrivate0, nodesPrivateResult0},
                new object[] { roleIdentity1, nodesToPrivate1, nodesPrivateResult1},
                //new object[] { roleIdentity2, nodesToPrivate2, nodesPrivateResult2},

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
            IResponse<string> response = await _privateAndPublicService.PublicNodeAsync(nodes);

            // Assert
            Assert.Equal(expected.Data, response.Data);

        }


        public static IEnumerable<object[]> PublicNodeData()
        {



            //user is guest 
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "guest", "guest", "");
            var nodesToPublic0 = new List<long>();
            IResponse<string> nodesPublicResult0 = new PublicResponse<string>("401: Server: No active session found. Please login and try again.", null, 400, false);


            //user is not guest
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "viet.nguyen03@student.csulb.edu", "user", "75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6");
            var nodesToPublic1 = new List<long> { 24, 25 };
            IResponse<string> nodesPublicResult1 = new PublicResponse<string>("", "200: Server: Public Node Success", 200, true);






            return new[]
            {

                new object[] { roleIdentity0, nodesToPublic0, nodesPublicResult0},
                new object[] { roleIdentity1, nodesToPublic1, nodesPublicResult1},


            };

        }

        

        






    }
}
